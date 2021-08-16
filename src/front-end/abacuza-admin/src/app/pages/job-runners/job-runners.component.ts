import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DataTableDirective } from 'angular-datatables';
import { Subject, Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { JobRunner } from 'src/app/models/job-runner';
import { ClustersService } from 'src/app/services/clusters.service';
import { CommonDialogResult, CommonDialogType } from 'src/app/services/common-dialog/common-dialog-data-types';
import { CommonDialogService } from 'src/app/services/common-dialog/common-dialog.service';
import { ComponentDialogUsage } from 'src/app/services/component-dialog/component-dialog-options';
import { ComponentDialogService } from 'src/app/services/component-dialog/component-dialog.service';
import { JobRunnersService } from 'src/app/services/job-runners.service';
import { ToastService } from 'src/app/services/toast/toast.service';
import { CreateJobRunnerComponent } from '../create-job-runner/create-job-runner.component';

@Component({
  selector: 'app-job-runners',
  templateUrl: './job-runners.component.html',
  styleUrls: ['./job-runners.component.scss']
})
export class JobRunnersComponent implements OnInit, OnDestroy {

  dtTableOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  jobRunners: JobRunner[] | null = null;
  clusterTypes: string[] | null = [];
  subscriptions: Subscription[] = [];

  @ViewChild(DataTableDirective, { static: false })
  dtElement!: DataTableDirective;

  constructor(private jobRunnersService: JobRunnersService,
    private clustersService: ClustersService,
    private commonDialogService: CommonDialogService,
    private toastService: ToastService,
    private componentDialogService: ComponentDialogService,
    private router: Router) { }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.subscriptions?.forEach(s => s.unsubscribe());
  }

  ngOnInit(): void {
    this.dtTableOptions = {
      pagingType: 'full_numbers'
    };

    this.subscriptions.push(this.jobRunnersService.getJobRunners()
      .subscribe(response => {
        this.jobRunners = response.body;
        this.dtTrigger.next();
      }));

    this.subscriptions.push(this.clustersService.getClusterTypes()
      .subscribe(response => this.clusterTypes = response.body));

  }

  onAddClicked(): void {
    this.subscriptions.push(this.componentDialogService.open(CreateJobRunnerComponent, {
      clusterTypes: this.clusterTypes,
      jobRunner: {
        clusterType: this.clusterTypes![0]
      }
    }, {
      title: 'Create Job Runner',
      usage: ComponentDialogUsage.Create
    }).subscribe(result => {
      if (result) {
        const jr = result.jobRunner as JobRunner;
        this.subscriptions.push(this.jobRunnersService.createJobRunner(jr)
          .pipe(catchError(err => {
            this.toastService.error(err.error);
            return throwError(err);
          }))
          .subscribe(responseId => {
            this.toastService.success('Job runner created successfully.');
            this.router.navigate(['/job-runners/details', responseId])
          }));
      }
    }));
  }

  onDeleteClicked(event: any) {
    this.subscriptions.push(this.commonDialogService.open('Confirm', 'Are you sure you want to delete the selected Job Runner?', CommonDialogType.Confirm)
      .subscribe(dr => {
        switch (dr) {
          case CommonDialogResult.Yes:
            this.subscriptions.push(this.jobRunnersService.deleteJobRunner(event.id)
              .pipe(catchError(err => {
                this.toastService.error(`Delete failed. ${err.error}`);
                return throwError(err);
              }))
              .subscribe(() => {
                this.toastService.success('Job Runner was deleted successfully.');
                this.jobRunners = this.jobRunners?.filter(x => x.id !== event.id) ?? null;
                this.rerender();
              }));
            break;
        }
      }));
  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }
}
