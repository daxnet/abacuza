import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NbToastrService } from '@nebular/theme';
import { JobRunner } from 'app/models/job-runner';
import { S3File } from 'app/models/s3-file';
import { ClustersService } from 'app/services/clusters.service';
import { JobRunnersService } from 'app/services/job-runners.service';
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
  key: string;
  initFiles: S3File[];

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private clustersService: ClustersService,
    private jobRunnerService: JobRunnersService,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.jobRunnerService.getJobRunnerById(params.id)
        .subscribe(res => {
           this.jobRunnerEntity = res.body;
           this.key = `job-runners/${this.jobRunnerEntity.id}`;
           if (this.jobRunnerEntity.binaryFiles) {
             this.initFiles = this.jobRunnerEntity.binaryFiles;
           }
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
      .subscribe(() => {
        this.toastrService.success('Update job runner successfully.', 'Success');
      });
  }

  onFileAdded(event: S3File[]): void {
    if (event) {
      const files: S3File[] = event;
      if (files.length > 0) {
        this.jobRunnerService.addBinaryFiles(this.jobRunnerEntity.id, files)
          .pipe(catchError(err => {
            this.toastrService.warning(`Files uploaded but job runner updated failed with message: ${err.message}.`, 'Failed to update job runner');
            return throwError(err.message);
          }))
          .subscribe(res => {
            this.jobRunnerEntity.binaryFiles = res.binaryFiles;
            this.toastrService.success('Files added successfully.', 'Success');
          });
      }
    }
  }

  onFileDeleted(event: S3File): void {
    if (event) {
      const file: S3File = event;
      this.jobRunnerService.deleteBinaryFile(this.jobRunnerEntity.id, file)
      .pipe(catchError(err => {
        this.toastrService.danger(`Error message: ${err.message}`, 'Delete binary file failed');
        return throwError(err.message);
      }))
      .subscribe(() => {
        this.toastrService.success('Binary file deleted successfully.', 'Success');
      })
    }
  }
}
