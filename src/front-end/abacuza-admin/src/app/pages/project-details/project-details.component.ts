import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProjectsService } from 'src/app/services/projects.service';
import { Project } from 'src/app/models/project';
import { JobRunnersService } from 'src/app/services/job-runners.service';
import { JobRunner } from 'src/app/models/job-runner';
import { EndpointsService } from 'src/app/services/endpoints.service';
import { Endpoint } from 'src/app/models/endpoint';

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss']
})
export class ProjectDetailsComponent implements OnInit {

  private subscriptions: Subscription[] = [];
  project: Project | null = null;
  jobRunners: JobRunner[] | null = [];
  inputEndpoints: Endpoint[] | null = [];
  outputEndpoints: Endpoint[] | null = [];

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private projectsService: ProjectsService,
    private jobRunnersService: JobRunnersService,
    private endpointsService: EndpointsService) { }

  ngOnInit(): void {
    this.subscriptions.push(this.activatedRoute.params.subscribe(params => {
      this.subscriptions.push(this.projectsService.getProjectById(params.id)
        .subscribe(response => {
          this.project = response.body;
        }));
      this.subscriptions.push(this.jobRunnersService.getJobRunners()
        .subscribe(response => {
          this.jobRunners = response.body;
        }));
      this.subscriptions.push(this.endpointsService.getEndpoints('input')
        .subscribe(response => {
          this.inputEndpoints = response.body;
        }));
      this.subscriptions.push(this.endpointsService.getEndpoints('output')
        .subscribe(response => {
          this.outputEndpoints = response.body;
        }))
    }));
  }

  close(): void {
    this.router.navigate(['/projects']);
  }
}
