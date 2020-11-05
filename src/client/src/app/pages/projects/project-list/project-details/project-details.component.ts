import { Component, ComponentFactoryResolver, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NbToastrService } from '@nebular/theme';
import { Endpoint } from 'app/models/endpoint';
import { JobRunner } from 'app/models/job-runner';
import { Project } from 'app/models/project';
import { EndpointsService } from 'app/services/endpoints.service';
import { JobRunnersService } from 'app/services/job-runners.service';
import { ProjectsService } from 'app/services/projects.service';
import { UIComponentBase } from 'app/ui-components/uicomponent-base';
import { UIComponentsHostDirective } from 'app/ui-components/uicomponents-host.directive';
import { UIComponentsProviderService } from 'app/ui-components/uicomponents-provider.service';
import { Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'ngx-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss'],
})
export class ProjectDetailsComponent implements OnInit, OnDestroy {

  @ViewChild(UIComponentsHostDirective, { static: true }) ngxUIComponentsHost: UIComponentsHostDirective;

  projectEntity: Project;
  inputEndpointEntity: Endpoint;
  jobRunnerEntity: JobRunner;
  updatingProject: { description: string } = <any>{};
  private componentEventSubscriptions: Subscription[] = [];

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private projectsService: ProjectsService,
    private endpointsService: EndpointsService,
    private jobRunnersService: JobRunnersService,
    private toastrService: NbToastrService,
    private componentsProvider: UIComponentsProviderService,
    private componentFactoryResolver: ComponentFactoryResolver,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.projectsService.getProjectById(params.id)
        .pipe(catchError(err => {
          this.toastrService.danger(err.message, 'Failed to retrieve project information');
          return throwError(err.message);
        }))
        .subscribe(res => {
          this.projectEntity = res.body;
          this.projectEntity.uiComponentData = this.projectEntity.inputEndpointSettings
            ?
            JSON.parse(this.projectEntity.inputEndpointSettings)
            :
            [];
          this.updatingProject.description = this.projectEntity.description;
          this.endpointsService.getEndpointByName(this.projectEntity.inputEndpointName)
            .pipe(catchError(err => {
              this.toastrService.danger(err.message, 'Failed to retrieve endpoint information');
              return throwError;
            }))
            .subscribe(ep => {
              this.inputEndpointEntity = ep.body;
              this.loadInputEndpointUIComponents(this.inputEndpointEntity);
            });
          this.jobRunnersService.getJobRunnerById(this.projectEntity.jobRunnerId)
            .pipe(catchError(err => {
              this.toastrService.danger(err.message, 'Failed to retrieve the job runner information');
              return throwError(err.message);
            }))
            .subscribe(jr => {
              this.jobRunnerEntity = jr.body;
            });
        });
    });
  }

  loadInputEndpointUIComponents(inputEndpoint: Endpoint): void {
    const viewContainerRef = this.ngxUIComponentsHost.viewContainerRef;
    viewContainerRef.clear();
    inputEndpoint.configurationUIElements.forEach(e => {
      const name = e['_type'];
      const item = this.componentsProvider.getRegisteredUIComponents().find(x => x.name === name);
      if (item) {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
        const componentRef = viewContainerRef.createComponent<UIComponentBase>(componentFactory);
        componentRef.instance.attributes = e;
        const data = this.projectEntity.uiComponentData.find(d => d.name === e.name);
        if (data) {
          componentRef.instance.value = data.value;
        }

        this.componentEventSubscriptions.push(componentRef.instance.modelChange.subscribe(event => {
          const uiComponentData = this.projectEntity.uiComponentData.find(d => d.name === event.component);
          if (!uiComponentData) {
            this.projectEntity.uiComponentData.push({
              name: event.component,
              value: event.data,
            });
          } else {
            uiComponentData.value = event.data;
          }
        }));
      }
    });
  }

  back(): void {
    this.router.navigate(['/pages/projects/project-list']);
  }

  save(): void {
    if (this.projectEntity.uiComponentData) {
      this.projectEntity.inputEndpointSettings = JSON.stringify(this.projectEntity.uiComponentData);
    }

    this.projectsService.updateProject(this.projectEntity.id, this.projectEntity)
      .pipe(catchError(err => {
        this.toastrService.danger(`Server responded error message: ${err.message}`, 'Failed to update project');
        return throwError(err.message);
      }))
      .subscribe(res => {
        this.toastrService.success('Project updated successfully.', 'Success');
      });
  }

  ngOnDestroy(): void {
    this.componentEventSubscriptions.forEach(s => s.unsubscribe());
  }
}
