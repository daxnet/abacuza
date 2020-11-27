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
import { UIComponentsHostDirective } from 'app/ui-components/uicomponents-host.directive';
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

  @ViewChild(UIComponentsHostDirective, { static: true }) ngxUIComponentsHost: UIComponentsHostDirective;

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
      // jobCancelledDate: {
      //   title: 'Cancelled At',
      //   type: 'custom',
      //   renderComponent: SmartTableDateCellRenderComponent,
      // },
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
        // { 
        //   name: 'ourCustomAction2', 
        //   title: '<i class="eva eva-github"></i>' 
        // },
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
  jobRunnerEntity: JobRunner;
  updatingProject: { description: string } = <any>{};
  revisionsSource: LocalDataSource = new LocalDataSource();
  isRevisionsTabActive: boolean = false;

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

  private loadInputEndpointUIComponents(inputEndpoint: Endpoint): void {
    const viewContainerRef = this.ngxUIComponentsHost.viewContainerRef;
    viewContainerRef.clear();
    inputEndpoint.configurationUIElements.forEach(e => {
      const name = e['_type'];
      const item = this.componentsProvider.getRegisteredUIComponents().find(x => x.name === name);
      if (item) {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
        const componentRef = viewContainerRef.createComponent<UIComponentBase>(componentFactory);
        componentRef.instance.attributes = e;
        componentRef.instance.attributes.contextualEntityId = this.projectEntity.id;
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

  submit(): void {
    // firstly save the current project.
    if (this.projectEntity.uiComponentData) {
      this.projectEntity.inputEndpointSettings = JSON.stringify(this.projectEntity.uiComponentData);
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
          })
      });

    this.isRevisionsTabActive = true;
    this.startTimer();
  }

  onChangeTab(event: { tabTitle: string; }): void {
    this.isRevisionsTabActive = event.tabTitle === 'Revisions';
    if (this.isRevisionsTabActive) {
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
