import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject, Subscription } from 'rxjs';
import { JobRunner } from 'src/app/models/job-runner';
import { ClustersService } from 'src/app/services/clusters.service';
import { CommonDialogService } from 'src/app/services/common-dialog/common-dialog.service';
import { ComponentDialogService } from 'src/app/services/component-dialog/component-dialog.service';
import { JobRunnersService } from 'src/app/services/job-runners.service';

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
    private componentDialogService: ComponentDialogService) { }

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

  onEditClicked(event: any) {

  }

  onDeleteClicked(event: any) {

  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }
}
