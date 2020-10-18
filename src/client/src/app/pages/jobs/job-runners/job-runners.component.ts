import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NbDialogService, NbToastrService } from '@nebular/theme';
import { CommonDialogResult } from 'app/models/common-dialog-result';
import { JobRunner } from 'app/models/job-runner';
import { ClustersService } from 'app/services/clusters.service';
import { CommonDialogService } from 'app/services/common-dialog.service';
import { JobRunnersService } from 'app/services/job-runners.service';
import { LocalDataSource } from 'ng2-smart-table';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CreateJobRunnerComponent } from './create-job-runner/create-job-runner.component';

@Component({
  selector: 'ngx-job-runners',
  templateUrl: './job-runners.component.html',
  styleUrls: ['./job-runners.component.scss'],
})
export class JobRunnersComponent implements OnInit {

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
      clusterType: {
        title: 'Cluster Type',
        type: 'text',
      },
    },
    actions: {
      position: 'right',
    },
    mode: 'external',
  };

  source: LocalDataSource = new LocalDataSource();
  clusterTypes: string[] = [];

  constructor(private jobRunnersService: JobRunnersService,
    private clustersService: ClustersService,
    private dialogService: NbDialogService,
    private toastrService: NbToastrService,
    private commonDialogService: CommonDialogService,
    private router: Router) { }

  ngOnInit(): void {
    this.jobRunnersService.getAllJobRunners()
      .pipe(catchError(err => {
          this.toastrService.danger(`Server responded with the error message: ${err.message}`,
          'Failed to load job runners', {
          duration: 6000,
        });
        return throwError(err.message);
      }))
      .subscribe(response => this.source.load(response.body));
    this.clustersService.getAllClusterTypes()
      .subscribe(response => this.clusterTypes = response.body);
  }

  onCreate(): void {
    this.dialogService.open(CreateJobRunnerComponent, {
      context: {
        clusterTypes: this.clusterTypes,
        jobRunnerEntity: new JobRunner(),
      },
      closeOnBackdropClick: false,
    })
    .onClose
    .subscribe(res => {
      if (res) {
        this.jobRunnersService.createJobRunner(res)
          .pipe(catchError(err => {
            this.toastrService.danger(`Server responded with the error message: ${err.message}`,
              'Failed to create cluster connection', {
              duration: 6000,
            });
            return throwError(err);
          }))
          .subscribe(responseId => {
            this.toastrService.success('Job runner created successfully.', 'Success');
            this.router.navigate(['/pages/jobs/job-runner-details', responseId]);
          });
      }
    });
  }

  onEdit(event): void {
    this.router.navigate(['/pages/jobs/job-runner-details', event.data.id]);
  }

  onDelete(event): void {
    this.commonDialogService.confirm('Delete Job Runner', 'Are you sure to delete the current job runner?')
    .subscribe(dr => {
      if (dr === CommonDialogResult.Yes) {
        this.jobRunnersService.deleteJobRunner(event.data.id)
          .pipe(catchError(err => {
            this.toastrService.danger(`Error message: ${err.message}`, 'Failed to delete the job runner');
            return throwError(err.message);
          }))
          .subscribe(_ => {
            this.toastrService.success('Job runner deleted successfully.', 'Success');
              this.source.remove(event.data);
              this.source.refresh();
          });
      }
    });
  }
}
