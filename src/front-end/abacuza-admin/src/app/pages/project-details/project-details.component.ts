import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, throwError } from 'rxjs';
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

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss']
})
export class ProjectDetailsComponent implements OnInit {

  private subscriptions: Subscription[] = [];
  project: Project | null = null;
  jobRunners: JobRunner[] | null = [];
  inputEndpoints: Endpoint[] | null = [];
  selectedAddingEndpointName?: string;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private projectsService: ProjectsService,
    private jobRunnersService: JobRunnersService,
    private endpointsService: EndpointsService,
    private toastService: ToastService) { }

  ngOnInit(): void {
    this.subscriptions.push(this.activatedRoute.params.subscribe(params => {
      this.subscriptions.push(this.projectsService.getProjectById(params.id)
        .subscribe(response => {
          this.project = response.body;
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
    }));
  }

  onAddInputEndpointClicked(): void {
    if (this.selectedAddingEndpointName) {
      const id = Guid.create().toString();
      const inputEndpointDefinition = new ProjectEndpointDefinition(id, this.selectedAddingEndpointName);
      this.project?.inputEndpoints.push(inputEndpointDefinition);
    }
  }

  onInputEndpointSettingsChange(event: EndpointSettingsChangedEvent) {
    console.log(event);
    const inputEndpoint = this.project?.inputEndpoints.find(ie => ie.id === event.endpointDefinitionId);
    if (inputEndpoint) {
      if (!inputEndpoint.settingsObject) {
        inputEndpoint.settingsObject = [];
      }

      const uiComponentData = inputEndpoint.settingsObject?.find(d => d.component === event.component);
      if (uiComponentData) {
        uiComponentData.value = event.data;
      } else {
        inputEndpoint.settingsObject?.push({
          component: event.component,
          value: event.data
        });
      }

      inputEndpoint.settings = JSON.stringify(inputEndpoint.settingsObject);
    }

    console.log(this.project);
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

  getSelectedInputEndpointTitle(definition: ProjectEndpointDefinition): string | null {
    return this.inputEndpoints?.find(ie => ie.name === definition.name)?.displayName ?? null;
  }

  close(): void {
    this.router.navigate(['/projects']);
  }
}
