import { AfterViewInit, Component, ComponentFactoryResolver, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NbToastrService } from '@nebular/theme';
import { SmartTableDateCellRenderComponent } from 'app/components/smart-table-date-cell-render/smart-table-date-cell-render.component';
import { SmartTableJobStatusRenderComponent } from 'app/components/smart-table-job-status-render/smart-table-job-status-render.component';
import { Endpoint } from 'app/models/endpoint';
import { JobRunner } from 'app/models/job-runner';
import { Project } from 'app/models/project';
import { EndpointsService } from 'app/services/endpoints.service';
import { JobRunnersService } from 'app/services/job-runners.service';
import { ProjectsService } from 'app/services/projects.service';
import { TextMessageDialogService } from 'app/services/text-message-dialog.service';
import { UIComponentBase } from 'app/ui-components/uicomponent-base';
import { UIComponentsInputEndpointHostDirective } from 'app/ui-components/uicomponents-inputendpoint-host.directive';
import { UIComponentsOutputEndpointHostDirective } from 'app/ui-components/uicomponents-outputendpoint-host.directive';
import { UIComponentsProviderService } from 'app/ui-components/uicomponents-provider.service';
import { LocalDataSource } from 'ng2-smart-table';
import { Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'ngx-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss'],
})
export class ProjectDetailsComponent implements OnInit, OnDestroy {

  @ViewChild(UIComponentsInputEndpointHostDirective, { static: true })
  ngxUIComponentsInputEndpointHost: UIComponentsInputEndpointHostDirective;

  @ViewChild(UIComponentsOutputEndpointHostDirective, { static: true })
  ngxUIComponentsOutputEndpointHost: UIComponentsOutputEndpointHostDirective;

  revisionsTableSettings = {
    columns: {
      createdDate: {
        title: 'Created At',
        type: 'custom',
        renderComponent: SmartTableDateCellRenderComponent,
      },
      jobSubmissionName: {
        title: 'Job Ref',
        type: 'text',
      },
      jobStatusName: {
        title: 'Job Status',
        type: 'custom',
        renderComponent: SmartTableJobStatusRenderComponent,
      },
    },
    actions: {
      add: false,
      edit: false,
      delete: false,
      custom: [
        {
          name: 'viewLogs',
          title: '<i class="eva eva-file-text-outline" title="View logs"></i>',
        },
      ],
      position: 'right',
    },
    mode: 'external',
    pager: {
      perPage: 8,
    },
  };

  timerId: any = null;
  projectEntity: Project;
  inputEndpointEntity: Endpoint;
  outputEndpointEntity: Endpoint;
  jobRunnerEntity: JobRunner;
  inputEndpoints: Endpoint[];
  outputEndpoints: Endpoint[];
  updatingProject: { description: string } = <any>{};
  revisionsSource: LocalDataSource = new LocalDataSource();
  selectedActiveTab: string = 'Input';

  private componentEventSubscriptions: Subscription[] = [];

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private projectsService: ProjectsService,
    private endpointsService: EndpointsService,
    private jobRunnersService: JobRunnersService,
    private toastrService: NbToastrService,
    private componentsProvider: UIComponentsProviderService,
    private textMessageDialogService: TextMessageDialogService,
    private componentFactoryResolver: ComponentFactoryResolver,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.endpointsService.getAvailableEndpoints('input')
        .pipe(catchError(err => {
          this.toastrService.danger(err.message, 'Failed to retrieve input endpoint list.');
          return throwError(err.message);
        }))
        .subscribe(res => this.inputEndpoints = res.body);
      this.endpointsService.getAvailableEndpoints('output')
        .pipe(catchError(err => {
          this.toastrService.danger(err.message, 'Failed to retrieve output endpoint list.');
          return throwError(err.message);
        }))
        .subscribe(res => this.outputEndpoints = res.body);

      this.projectsService.getProjectById(params.id)
        .pipe(catchError(err => {
          this.toastrService.danger(err.message, 'Failed to retrieve project information');
          return throwError(err.message);
        }))
        .subscribe(res => {
          this.projectEntity = res.body;
          this.projectEntity.inputEndpointUIComponentData = this.projectEntity.inputEndpointSettings
            ?
            JSON.parse(this.projectEntity.inputEndpointSettings)
            :
            [];
          this.projectEntity.outputEndpointUIComponentData = this.projectEntity.outputEndpointSettings
            ?
            JSON.parse(this.projectEntity.outputEndpointSettings)
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
              this.loadEndpointUIComponents(
                this.inputEndpointEntity,
                this.ngxUIComponentsInputEndpointHost,
                (projEntity) => projEntity.inputEndpointUIComponentData);
            });
          this.endpointsService.getEndpointByName(this.projectEntity.outputEndpointName)
            .pipe(catchError(err => {
              this.toastrService.danger(err.message, 'Failed to retrieve endpoint information');
              return throwError;
            }))
            .subscribe(ep => {
              this.outputEndpointEntity = ep.body;
              this.loadEndpointUIComponents(
                this.outputEndpointEntity,
                this.ngxUIComponentsOutputEndpointHost,
                (projEntity) => projEntity.outputEndpointUIComponentData);
            });
          this.jobRunnersService.getJobRunnerById(this.projectEntity.jobRunnerId)
            .pipe(catchError(err => {
              this.toastrService.danger(err.message, 'Failed to retrieve the job runner information');
              return throwError(err.message);
            }))
            .subscribe(jr => {
              this.jobRunnerEntity = jr.body;
            });

          // firstly update the revisions list
          this.updateRevisions(this.projectsService, this.projectEntity.id, this.revisionsSource);
        });
    });
  }

  private startTimer(): void {
    this.stopTimer();
    this.timerId = setInterval(this.updateRevisions,
      5000,
      this.projectsService,
      this.projectEntity.id,
      this.revisionsSource);
  }

  private stopTimer(): void {
    if (this.timerId) {
      clearInterval(this.timerId);
    }
  }

  private updateRevisions(ps: ProjectsService, projectId: string, revisionsSource: LocalDataSource): void {
    ps.getRevisions(projectId)
      .subscribe(revisions => {
        revisionsSource.load(revisions);
      });
  }

  private loadEndpointUIComponents(
      endpoint: Endpoint,
      directive: UIComponentsInputEndpointHostDirective | UIComponentsOutputEndpointHostDirective,
      uiComponentDataFunc: (projectEntity: Project) => any): void {
    const viewContainerRef = directive.viewContainerRef;
    viewContainerRef.clear();
    endpoint.configurationUIElements.forEach(e => {
      const name = e['_type'];
      const id = e['_endpoint'] + '.' + e['name'];
      const item = this.componentsProvider.getRegisteredUIComponents().find(x => x.name === name);
      if (item) {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
        const componentRef = viewContainerRef.createComponent<UIComponentBase>(componentFactory);
        componentRef.instance.attributes = e;
        componentRef.instance.id = id;
        componentRef.instance.attributes.contextualEntityId = this.projectEntity.id;
        const data = uiComponentDataFunc(this.projectEntity).find(d => d.id === id);
        if (data) {
          componentRef.instance.value = data.value;
        } else if (e['defaultValueObject']) {
          componentRef.instance.value = e['defaultValueObject'];
        }

        this.componentEventSubscriptions.push(componentRef.instance.modelChange.subscribe(event => {
          const uiComponentData = uiComponentDataFunc(this.projectEntity).find(d => d.id === event.component);
          if (!uiComponentData) {
            uiComponentDataFunc(this.projectEntity).push({
              id: event.component,
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
    if (this.projectEntity.inputEndpointUIComponentData) {
      this.projectEntity.inputEndpointSettings = JSON.stringify(this.projectEntity.inputEndpointUIComponentData);
    }

    if (this.projectEntity.outputEndpointUIComponentData) {
      this.projectEntity.outputEndpointSettings = JSON.stringify(this.projectEntity.outputEndpointUIComponentData);
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

  submit(): void {
    // firstly save the current project.
    if (this.projectEntity.inputEndpointUIComponentData) {
      this.projectEntity.inputEndpointSettings = JSON.stringify(this.projectEntity.inputEndpointUIComponentData);
    }

    if (this.projectEntity.outputEndpointUIComponentData) {
      this.projectEntity.outputEndpointSettings = JSON.stringify(this.projectEntity.outputEndpointUIComponentData);
    }

    this.projectsService.updateProject(this.projectEntity.id, this.projectEntity)
      .pipe(catchError(err => {
        this.toastrService.danger(`Server responded error message: ${err.message}`, 'Failed to update project');
        return throwError(err.message);
      }))
      .subscribe(_ => {
        // then create the revision.
        this.projectsService.createRevision(this.projectEntity.id)
          .pipe(catchError(err => {
            this.toastrService.danger(err.message, 'Failed to create the revision');
            return throwError(err.message);
          }))
          .subscribe(revisionId => {
            this.toastrService.success(`Revision created with ID: ${revisionId}`, 'Success');
          });
      });

    this.selectedActiveTab = 'Revisions';
    this.startTimer();
  }

  onChangeTab(event: { tabTitle: string; }): void {
    this.selectedActiveTab = event.tabTitle;
    if (this.selectedActiveTab === 'Revisions') {
      this.startTimer();
    } else {
      this.stopTimer();
    }
  }

  onRevisionsCustomAction(event: any): void {
    switch (event.action) {
      case 'viewLogs':
        this.showLogs(event.data.id);
      break;
    }
  }

  onInputEndpointSelectedChange(event: any): void {
    this.endpointsService.getEndpointByName(event)
      .pipe(catchError(err => {
        this.toastrService.danger(err.message, 'Failed to retrieve endpoint information');
        return throwError;
      }))
      .subscribe(ep => {
        this.inputEndpointEntity = ep.body;
        this.loadEndpointUIComponents(
          this.inputEndpointEntity,
          this.ngxUIComponentsInputEndpointHost,
          (projEntity) => projEntity.inputEndpointUIComponentData);
      });
  }

  onOutputEndpointSelectedChange(event: any): void {
    this.endpointsService.getEndpointByName(event)
        .pipe(catchError(err => {
          this.toastrService.danger(err.message, 'Failed to retrieve endpoint information');
          return throwError;
        }))
        .subscribe(ep => {
          this.outputEndpointEntity = ep.body;
          this.loadEndpointUIComponents(
            this.outputEndpointEntity,
            this.ngxUIComponentsOutputEndpointHost,
            (projEntity) => projEntity.outputEndpointUIComponentData);
        });
  }

  showLogs(revisionId: string): void {
    this.projectsService.getRevisionLogs(revisionId)
      .subscribe(logEntries => {
        let log = '';
        logEntries.forEach(le => log = log.concat(le).concat('\r\n'));
        this.textMessageDialogService.show('Revision Log', log);
      });
  }

  ngOnDestroy(): void {
    this.stopTimer();
    this.componentEventSubscriptions.forEach(s => s.unsubscribe());
  }
}
