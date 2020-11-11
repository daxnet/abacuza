import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NbDialogService, NbToastrService } from '@nebular/theme';
import { SmartTableDateCellRenderComponent } from 'app/components/smart-table-date-cell-render/smart-table-date-cell-render.component';
import { CommonDialogResult } from 'app/models/common-dialog-result';
import { Endpoint } from 'app/models/endpoint';
import { JobRunner } from 'app/models/job-runner';
import { Project } from 'app/models/project';
import { CommonDialogService } from 'app/services/common-dialog.service';
import { EndpointsService } from 'app/services/endpoints.service';
import { JobRunnersService } from 'app/services/job-runners.service';
import { ProjectsService } from 'app/services/projects.service';
import { LocalDataSource } from 'ng2-smart-table';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CreateProjectComponent } from './create-project/create-project.component';

@Component({
  selector: 'ngx-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.scss'],
})
export class ProjectListComponent implements OnInit {

  settings = {
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
      saveButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      name: {
        title: 'Name',
        type: 'text',
      },
      description: {
        title: 'Description',
        type: 'text',
      },
      inputEndpointName: {
        title: 'Input Endpoint',
        type: 'text',
      },
      dateCreated: {
        title: 'Date Created',
        type: 'custom',
        renderComponent: SmartTableDateCellRenderComponent,
      },
    },
    actions: {
      position: 'right',
    },
    mode: 'external',
  };

  source: LocalDataSource = new LocalDataSource();
  inputEndpoints: Endpoint[] = [];
  jobRunners: JobRunner[] = [];

  constructor(private projectsService: ProjectsService,
    private endpointsService: EndpointsService,
    private jobRunnersService: JobRunnersService,
    private dialogService: NbDialogService,
    private commonDialogService: CommonDialogService,
    private toastrService: NbToastrService,
    private router: Router) { }

  ngOnInit(): void {
    this.projectsService.getAllProjects()
      .pipe(catchError(err => {
        this.toastrService.danger(`Server responded with the error message: ${err.message}`,
          'Failed to load job runners');
        return throwError(err.message);
      }))
      .subscribe(res => {
        const projects: Project[] = res.body;
        this.source.load(projects);
      });

    this.endpointsService.getAvailableEndpoints('input')
      .subscribe(res => {
        this.inputEndpoints = res.body;
      });

    this.jobRunnersService.getAllJobRunners()
      .subscribe(res => {
        this.jobRunners = res.body;
      });
  }

  onCreate(): void {
    this.dialogService.open(CreateProjectComponent, {
      context: {
        inputEndpoints: this.inputEndpoints,
        jobRunners: this.jobRunners,
        projectEntity: new Project(),
      },
    })
    .onClose
    .subscribe(model => {
      if (model) {
        this.projectsService.createProject(model)
          .pipe(catchError(err => {
            this.toastrService.danger(`Server responded with error message: ${err.message}`,
              'Failed to create project');
            return throwError(err.message);
          }))
          .subscribe(id => {
            this.toastrService.success('Project created successfully.', 'Success');
            this.router.navigate(['/pages/projects/project-details', id]);
          });
      }
    });
  }

  onEdit(event): void {
    this.router.navigate(['/pages/projects/project-details', event.data.id]);
  }

  onDelete(event): void {
    this.commonDialogService.confirm('Delete Project', 'Are you sure you want to delete the selected project?')
      .subscribe(dr => {
        if (dr === CommonDialogResult.Yes) {
          this.projectsService.deleteProject(event.data.id)
            .pipe(catchError(err => {
              this.toastrService.danger(`Server responded with error message: ${err.message}`, 'Failed to delete project');
              return throwError(err.message);
            }))
            .subscribe(res => {
              this.source.remove(event.data);
              this.source.refresh();
              this.toastrService.success('Project deleted successfully.', 'Success');
            });
        }
      });
  }
}
