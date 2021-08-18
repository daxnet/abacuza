import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject, Subscription, throwError } from 'rxjs';
import { Project } from 'src/app/models/project';
import { ProjectsService } from 'src/app/services/projects.service';
import { JobRunner } from 'src/app/models/job-runner';
import { JobRunnersService } from 'src/app/services/job-runners.service';
import { CommonDialogService } from 'src/app/services/common-dialog/common-dialog.service';
import { CreateProjectComponent } from '../create-project/create-project.component';
import { ComponentDialogUsage } from 'src/app/services/component-dialog/component-dialog-options';
import { ComponentDialogService } from 'src/app/services/component-dialog/component-dialog.service';
import { Endpoint } from 'src/app/models/endpoint';
import { EndpointsService } from 'src/app/services/endpoints.service';
import { catchError } from 'rxjs/operators';
import { ToastService } from 'src/app/services/toast/toast.service';
import { Router } from '@angular/router';
import { CommonDialogType, CommonDialogResult } from 'src/app/services/common-dialog/common-dialog-data-types';
import { Guid } from 'guid-typescript';
import { ProjectEndpointDefinition } from 'src/app/models/project-endpoint-definition';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit, OnDestroy {

  dtTableOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  projects: Project[] | null = null;
  jobRunners: JobRunner[] | null = null;
  inputEndpoints: Endpoint[] | null = null;
  outputEndpoints: Endpoint[] | null = null;
  
  private subscriptions: Subscription[] = [];

  @ViewChild(DataTableDirective, { static: false })
  dtElement!: DataTableDirective;
  
  constructor(private router: Router,
    private projectsService: ProjectsService,
    private jobRunnersService: JobRunnersService,
    private endpointsService: EndpointsService,
    private toastService: ToastService,
    private componentDialogService: ComponentDialogService,
    private commonDialogService: CommonDialogService) { }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.subscriptions?.forEach(s => s.unsubscribe());
  }

  ngOnInit(): void {
    this.dtTableOptions = {
      responsive: true,
      pagingType: 'full_numbers'
    };

    this.subscriptions.push(this.projectsService.getProjects()
      .subscribe(response => {
        this.projects = response.body;
        this.dtTrigger.next();
      }));

    this.subscriptions.push(this.jobRunnersService.getJobRunners()
      .subscribe(response => {
        this.jobRunners = response.body;
      }));

    this.subscriptions.push(this.endpointsService.getEndpoints('input')
      .subscribe(response => {
        this.inputEndpoints = response.body;
      }));

    this.subscriptions.push(this.endpointsService.getEndpoints('output')
      .subscribe(response => {
        this.outputEndpoints = response.body;
      }));
  }

  getProjectInputEndpointDescription(proj: Project): string {
    return proj.inputEndpoints && proj.inputEndpoints.length > 0 ? (proj.inputEndpoints.length > 1 ? '(multiple)' : proj.inputEndpoints[0].name) : '(not defined)';
  }

  getProjectOutputEndpointDescription(proj: Project): string {
    if (proj.selectedOutputEndpointId && proj.outputEndpoints && proj.outputEndpoints.length > 0) {
      const outputEndpointDefinition = proj.outputEndpoints?.find(oe => oe.id === proj.selectedOutputEndpointId);
      if (outputEndpointDefinition) {
        return outputEndpointDefinition.name;
      }
    }

    return '(not defined)';
  }

  onAddClicked(): void {
    this.subscriptions.push(this.componentDialogService.open(CreateProjectComponent, {
      jobRunners: this.jobRunners,
      inputEndpoints: this.inputEndpoints,
      outputEndpoints: this.outputEndpoints,
      selectedInputEndpointName: this.inputEndpoints ? this.inputEndpoints[0].name : '',
      selectedOutputEndpointName: this.outputEndpoints ? this.outputEndpoints[0].name : '',
      project: {
        jobRunnerId: this.jobRunners ? this.jobRunners[0].id : ''
      }
    }, {
      title: 'Add Project',
      usage: ComponentDialogUsage.Create
    }).subscribe(result => {
      if (result) {
        const inputEndpointId = Guid.create().toString();
        const outputEndpointId = Guid.create().toString();
        const project: Project = {
          name: result.project.name,
          description: result.project.description,
          inputEndpoints: [new ProjectEndpointDefinition(
            inputEndpointId,
            result.selectedInputEndpointName
          )],
          selectedOutputEndpointId: outputEndpointId,
          outputEndpoints: [new ProjectEndpointDefinition(
            outputEndpointId,
            result.selectedOutputEndpointName
          )],
          jobRunnerId: result.project.jobRunnerId
        };

        this.subscriptions.push(this.projectsService.createProject(project)
          .pipe(catchError(err => {
            this.toastService.error(err.error);
            return throwError(err);
          }))
          .subscribe(responseId => {
            this.toastService.success('Project created successfully.');
            this.router.navigate(['/projects/details', responseId])
          }));
      }
    }));
  }

  onDeleteClicked(event: any) {
    this.subscriptions.push(this.commonDialogService.open('Confirm', 'Are you sure you want to delete the project?', CommonDialogType.Confirm)
      .subscribe(dr => {
        if (dr) {
          switch(dr) {
            case CommonDialogResult.Yes:
              const project = event as Project;
              if (project && project.id) {
                this.subscriptions.push(this.projectsService.deleteProject(project.id)
                  .pipe(catchError(err => {
                    this.toastService.error(err.error);
                    return throwError(err);
                  })).subscribe(() => {
                    this.toastService.success('Project was deleted successfully.');
                    this.projects = this.projects?.filter(x => x.id !== event.id) ?? null;
                    this.rerender();
                  }));
              }
              break;
          }
        }
      }));
  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }

}
