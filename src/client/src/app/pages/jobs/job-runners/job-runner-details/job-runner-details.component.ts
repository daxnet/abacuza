import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobRunner } from 'app/models/job-runner';
import { ClustersService } from 'app/services/clusters.service';
import { JobRunnersService } from 'app/services/job-runners.service';

@Component({
  selector: 'ngx-job-runner-details',
  templateUrl: './job-runner-details.component.html',
  styleUrls: ['./job-runner-details.component.scss'],
})
export class JobRunnerDetailsComponent implements OnInit {

  jobRunnerEntity: JobRunner;
  clusterTypes: string[] = [];

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private clustersService: ClustersService,
    private jobRunnerService: JobRunnersService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.jobRunnerService.getJobRunnerById(params.id)
        .subscribe(res => this.jobRunnerEntity = res.body);
    });

    this.clustersService.getAllClusterTypes()
      .subscribe(res => this.clusterTypes = res.body);
  }

  back(): void {
    this.router.navigate(['/pages/jobs/job-runners']);
  }
}
