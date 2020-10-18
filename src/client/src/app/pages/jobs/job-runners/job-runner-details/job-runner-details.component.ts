import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NbToastrService } from '@nebular/theme';
import { CommonDialogResult } from 'app/models/common-dialog-result';
import { JobRunner } from 'app/models/job-runner';
import { S3File } from 'app/models/s3-file';
import { ClustersService } from 'app/services/clusters.service';
import { CommonDialogService } from 'app/services/common-dialog.service';
import { FileUploadService } from 'app/services/file-upload.service';
import { JobRunnersService } from 'app/services/job-runners.service';
import { LocalDataSource } from 'ng2-smart-table';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'ngx-job-runner-details',
  templateUrl: './job-runner-details.component.html',
  styleUrls: ['./job-runner-details.component.scss'],
})
export class JobRunnerDetailsComponent implements OnInit {

  jobRunnerEntity: JobRunner;
  clusterTypes: string[] = [];

  binaryFilesTableSettings = {
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      file: {
        title: 'File name',
        type: 'text',
        width: '90%',
      },
    },
    actions: {
      edit: false,
      position: 'right',
    },
    mode: 'external',
  };

  source: LocalDataSource = new LocalDataSource();
  s3Files: S3File[] = [];

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private clustersService: ClustersService,
    private jobRunnerService: JobRunnersService,
    private toastrService: NbToastrService,
    private fileUploadService: FileUploadService,
    private commonDialogService: CommonDialogService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.jobRunnerService.getJobRunnerById(params.id)
        .subscribe(res => {
           this.jobRunnerEntity = res.body;
           if (this.jobRunnerEntity.binaryFiles) {
             this.s3Files = this.jobRunnerEntity.binaryFiles;
           }

           this.source.load(this.s3Files);
        });
    });

    this.clustersService.getAllClusterTypes()
      .subscribe(res => this.clusterTypes = res.body);
  }

  back(): void {
    this.router.navigate(['/pages/jobs/job-runners']);
  }

  save(): void {
    this.jobRunnerService.updateJobRunner(this.jobRunnerEntity.id, this.jobRunnerEntity)
      .pipe(catchError(err => {
        this.toastrService.danger(`Error message: ${err.message}`, 'Failed to update job runner');
        return throwError(err.message);
      }))
      .subscribe(res => {
        this.toastrService.success('Update job runner successfully.', 'Success');
        this.back();
      });
  }

  onAddFile(): void {
    this.fileUploadService.uploadFiles('data', `job-runners/${this.jobRunnerEntity.name}`, '.zip,.jar', false)
      .subscribe(files => {
        if (files) {
          this.jobRunnerService.addBinaryFiles(this.jobRunnerEntity.id, files)
            .pipe(catchError(err => {
              this.toastrService.warning(`Files uploaded but job runner updated failed with message: ${err.message}.`, 'Failed to update job runner');
              return throwError(err.message);
            }))
            .subscribe(res => {
              this.jobRunnerEntity.binaryFiles = res.binaryFiles;
              res.binaryFiles.forEach(bf => this.source.add(bf));
              this.source.refresh();
              this.toastrService.success('Files added successfully.', 'Success');
            });
        }
      });
  }

  onDeleteFile(event): void {
    // event.data is S3File
    this.commonDialogService.confirm('Delete binary file', `Are you sure to delete ${event.data.file}?`)
      .subscribe(dr => {
        if (dr === CommonDialogResult.Yes) {
          this.jobRunnerService.deleteBinaryFile(this.jobRunnerEntity.id, event.data)
            .pipe(catchError(err => {
              this.toastrService.danger(`Error message: ${err.message}`, 'Delete binary file failed');
              return throwError(err.message);
            }))
            .subscribe(res => {
              this.source.remove(event.data);
              this.source.refresh();
              this.toastrService.success('Binary file deleted successfully.', 'Success');
            });
        }
      });
  }
}
