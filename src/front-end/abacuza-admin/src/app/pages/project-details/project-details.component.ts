import { Component, OnInit, ChangeDetectorRef, ChangeDetectionStrategy, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription, throwError } from 'rxjs';
import { ProjectsService } from 'src/app/services/projects.service';
import { Project } from 'src/app/models/project';
import { JobRunnersService } from 'src/app/services/job-runners.service';
import { JobRunner } from 'src/app/models/job-runner';
import { EndpointsService } from 'src/app/services/endpoints.service';
import { Endpoint } from 'src/app/models/endpoint';
import { Guid } from 'guid-typescript';
import { ProjectEndpointDefinition } from 'src/app/models/project-endpoint-definition';
import { EndpointSettingsChangedEvent } from 'src/app/components/project-endpoint-editor/endpoint-settings-changed-event';
import { catchError } from 'rxjs/operators';
import { ToastService } from 'src/app/services/toast/toast.service';
import { cloneDeep } from 'lodash';
import { CommonDialogService } from 'src/app/services/common-dialog/common-dialog.service';
import { CommonDialogType, CommonDialogResult } from 'src/app/services/common-dialog/common-dialog-data-types';
import { ProjectRevision } from 'src/app/models/project-revision';
import { DataTableDirective } from 'angular-datatables';

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss']
})
export class ProjectDetailsComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = [];

  project: Project | null = null;
  jobRunners: JobRunner[] | null = [];
  inputEndpoints: Endpoint[] | null = [];
  outputEndpoints: Endpoint[] | null = [];
  revisions: ProjectRevision[] | null = [];
  selectedAddingEndpointName?: string;
  selectedOutputEndpointName?: string;
  selectedOutputEndpointDefinition?: ProjectEndpointDefinition;
  inputEndpointExpandAll: boolean = true;
  activeTabId: any;
  timerId: any;
  dtTableOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  @ViewChild(DataTableDirective, { static: false })
  dtElement!: DataTableDirective;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private projectsService: ProjectsService,
    private jobRunnersService: JobRunnersService,
    private endpointsService: EndpointsService,
    private toastService: ToastService,
    private comonDialogService: CommonDialogService) { }

  ngOnInit(): void {
    this.dtTableOptions = {
      responsive: true,
      pagingType: 'full_numbers'
    };

    this.subscriptions.push(this.activatedRoute.params.subscribe(params => {
      this.subscriptions.push(this.projectsService.getProjectById(params.id)
        .subscribe(response => {
          this.project = response.body;
          this.selectedOutputEndpointDefinition = this.project?.outputEndpoints.find(oed => oed.id === this.project?.selectedOutputEndpointId);
          if (this.selectedOutputEndpointDefinition) {
            this.selectedOutputEndpointName = this.selectedOutputEndpointDefinition.name;
          }
        }));
      this.subscriptions.push(this.jobRunnersService.getJobRunners()
        .subscribe(response => {
          this.jobRunners = response.body;
        }));
      this.subscriptions.push(this.endpointsService.getEndpoints('input')
        .subscribe(response => {
          this.inputEndpoints = response.body;
          if (this.inputEndpoints) {
            this.selectedAddingEndpointName = this.inputEndpoints[0].name;
          }
        }));
      this.subscriptions.push(this.endpointsService.getEndpoints('output')
        .subscribe(response => {
          this.outputEndpoints = response.body;
        }));
    }));
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  expandCollapseAll(accordion: any): void {
    if (this.inputEndpointExpandAll) {
      accordion.collapseAll();
    } else {
      accordion.expandAll();
    }

    this.inputEndpointExpandAll = !this.inputEndpointExpandAll;
  }

  onAddInputEndpointClicked(): void {
    if (this.selectedAddingEndpointName) {
      const id = Guid.create().toString();
      const inputEndpointDefinition = new ProjectEndpointDefinition(id, this.selectedAddingEndpointName);
      this.project?.inputEndpoints.push(inputEndpointDefinition);
    }
  }

  onRemoveInputEndpoint(event: ProjectEndpointDefinition): void {
    if (event) {
      this.subscriptions.push(this.comonDialogService.open('Confirm', 'Are you sure you want to delete the selected endpoint?', CommonDialogType.Confirm)
        .subscribe(dr => {
          if (dr) {
            switch(dr) {
              case CommonDialogResult.Yes:
                if (this.project) {
                  this.project.inputEndpoints = this.project?.inputEndpoints.filter(x => x.id !== event.id);
                }
                break;
            }
          }
        })
      );
    }
  }

  onEndpointSettingsChange(event: EndpointSettingsChangedEvent, type: string) {
    if (event) {
      const endpointDefinitions = type === 'input' ? this.project?.inputEndpoints : this.project?.outputEndpoints;
      const endpointDefinition = endpointDefinitions?.find(oe => oe.id === event.endpointDefinitionId);
      if (endpointDefinition) {
        if (!endpointDefinition.settingsObject) {
          endpointDefinition.settingsObject = [];
        }

        const uiComponentData = endpointDefinition.settingsObject?.find(d => d.component === event.component);
        if (uiComponentData) {
          uiComponentData.value = event.data;
        } else {
          endpointDefinition.settingsObject?.push({
            component: event.component,
            value: event.data
          });
        }

        endpointDefinition.settings = JSON.stringify(endpointDefinition.settingsObject);
      }
    }
  }

  onSelectedOutputEndpointNameChange(event: any): void {
    if (this.project && this.selectedOutputEndpointName) {
      let outputEndpointDefinition = this.project?.outputEndpoints.find(oed => oed.name === this.selectedOutputEndpointName);
      if (outputEndpointDefinition) {
        this.project.selectedOutputEndpointId = outputEndpointDefinition.id;
        this.selectedOutputEndpointDefinition = outputEndpointDefinition;
      } else {
        const id = Guid.create().toString();
        outputEndpointDefinition = new ProjectEndpointDefinition(id, this.selectedOutputEndpointName);
        this.project.selectedOutputEndpointId = id;
        this.project.outputEndpoints.push(outputEndpointDefinition);
        this.selectedOutputEndpointDefinition = outputEndpointDefinition;
      }
    }
  }

  save(close: boolean): void {
    if (this.project && this.project.id) {
      this.subscriptions.push(this.projectsService.updateProject(this.project.id, this.project)
        .pipe(catchError(err => {
          this.toastService.error(`Failed to update project. Error message: ${err.message}`);
          return throwError(err.message);
        }))
        .subscribe(() => {
          this.toastService.success('Update project successfully.');
          if (close) {
            this.close();
          }
        }));
    }
  }

  submit(): void {
    if (this.project && this.project.id) {
      this.subscriptions.push(this.projectsService.updateProject(this.project.id, this.project)
        .pipe(catchError(err => {
          this.toastService.error(`Failed to update project. Error message: ${err.message}`);
          return throwError(err.message);
        }))
        .subscribe(() => {
          this.subscriptions.push(this.projectsService.createRevision(this.project!.id!)
            .pipe(catchError(err => {
              this.toastService.error(err.message);
              return throwError(err.message);
            }))
            .subscribe(revisionId => {
              this.toastService.success(`Revision created: ${revisionId}`);
            })
          )
        })
      );
      this.activeTabId = 3;
      // this.startTimer();
    }
    
  }

  getSelectedInputEndpointTitle(definition: ProjectEndpointDefinition): string | null {
    return this.inputEndpoints?.find(ie => ie.name === definition.name)?.displayName ?? null;
  }

  close(): void {
    this.router.navigate(['/projects']);
  }

  activeIdChange(event: any): void {
    this.updateRevisions(this.project!, this.projectsService, this.subscriptions, this.revisions);
    if (event === 3) {
      this.startTimer();
    } else {
      this.stopTimer();
    }
  }

  private stopTimer(): void {
    if (this.timerId) {
      clearInterval(this.timerId);
    }
  }

  private startTimer(): void {
    this.stopTimer();
    this.timerId = setInterval(this.updateRevisions,
      5000, this.project, this.projectsService, this.subscriptions, this.revisions);
  }

  private updateRevisions(project: Project, 
    projectsService: ProjectsService, 
    subscriptions: Subscription[],
    revisions: ProjectRevision[] | null): void {
    if (project && project.id) {
      subscriptions.push(projectsService.getRevisions(project?.id)
        .subscribe(rev => {
          revisions = rev;
          this.rerender();
        }));
    }
    
  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }
}
