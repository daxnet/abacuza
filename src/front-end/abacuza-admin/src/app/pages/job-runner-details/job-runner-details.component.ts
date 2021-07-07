import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JsonEditorOptions } from 'ang-jsoneditor';
import { Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { JobRunner } from 'src/app/models/job-runner';
import { S3File } from 'src/app/models/s3-file';
import { ClustersService } from 'src/app/services/clusters.service';
import { JobRunnersService } from 'src/app/services/job-runners.service';
import { ToastService } from 'src/app/services/toast/toast.service';

@Component({
  selector: 'app-job-runner-details',
  templateUrl: './job-runner-details.component.html',
  styleUrls: ['./job-runner-details.component.scss']
})
export class JobRunnerDetailsComponent implements OnInit, OnDestroy {

  jobRunner: JobRunner | null = null;
  clusterTypes: string[] | null = [];
  jsonEditorOptions: JsonEditorOptions = new JsonEditorOptions();

  initFiles: S3File[] = [];
  key: string = '';

  private subscriptions: Subscription[] = [];

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private jobRunnersService: JobRunnersService,
    private clustersService: ClustersService,
    private toastService: ToastService) {
      this.jsonEditorOptions.mode = 'code';
      this.jsonEditorOptions.mainMenuBar = false;
    }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  ngOnInit(): void {
    this.subscriptions.push(this.activatedRoute.params.subscribe(params => {
      this.jobRunnersService.getJobRunnerById(params.id)
        .subscribe(response => {
          this.jobRunner = response.body;
          if (this.jobRunner !== null) {
            this.key = `job-runners/${this.jobRunner.id}`;
            if (this.jobRunner.binaryFiles) {
              this.initFiles = this.jobRunner.binaryFiles;
            }
          }
        })
    }));

    this.subscriptions.push(this.clustersService.getClusterTypes()
      .subscribe(response => this.clusterTypes = response.body));
  }

  onFileAdded(event: S3File[]): void {
    if (event) {
      const files: S3File[] = event;
      if (files.length > 0 && this.jobRunner) {
        this.subscriptions.push(this.jobRunnersService.addBinaryFiles(this.jobRunner.id, files)
          .pipe(catchError(err => {
            this.toastService.error('Files uploaded failed.');
            return throwError(err.message);
          }))
          .subscribe(response => {
            this.jobRunner!.binaryFiles = response.binaryFiles;
            this.toastService.success('Files added successfully.');
          }));
      }
    }
  }

  onFileDeleted(event: S3File): void {
    if (event && this.jobRunner) {
      const file: S3File = event;
      this.subscriptions.push(this.jobRunnersService.deleteBinaryFile(this.jobRunner.id, file)
        .pipe(catchError(err => {
          this.toastService.error(`Delete file failed. Error message: ${err.message}`);
          return throwError(err.message);
        }))
        .subscribe(() => {
          this.toastService.success('Binary file deleted successfully.');
      }));
    }
  }

  save(close: boolean): void {
    if (this.jobRunner) {
      this.subscriptions.push(this.jobRunnersService.updateJobRunner(this.jobRunner.id, this.jobRunner)
        .pipe(catchError(err => {
          this.toastService.error(`Failed to update job runner. Error message: ${err.message}`);
          return throwError(err.message);
        }))
        .subscribe(() => {
          this.toastService.success('Update job runner successfully.');
          if (close) {
            this.close();
          }
        }));
    }
  }

  close(): void {
    this.router.navigate(['/job-runners']);
  }
}
