import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JsonEditorOptions } from 'ang-jsoneditor';
import { Subscription } from 'rxjs';
import { JobRunner } from 'src/app/models/job-runner';
import { S3File } from 'src/app/models/s3-file';
import { ClustersService } from 'src/app/services/clusters.service';
import { JobRunnersService } from 'src/app/services/job-runners.service';

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
    private jobRunnersService: JobRunnersService,
    private clustersService: ClustersService) {
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
    console.log(event);
  }

  onFileDeleted(event: S3File): void {
    console.log(event);
  }
}
