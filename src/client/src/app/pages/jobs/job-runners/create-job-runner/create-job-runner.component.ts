import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { JobRunner } from 'app/models/job-runner';

@Component({
  selector: 'ngx-create-job-runner',
  templateUrl: './create-job-runner.component.html',
  styleUrls: ['./create-job-runner.component.scss'],
})
export class CreateJobRunnerComponent implements OnInit {

  @Input() clusterTypes: string[];
  @Input() jobRunnerEntity: JobRunner;

  constructor(protected ref: NbDialogRef<CreateJobRunnerComponent>) { }

  ngOnInit(): void {
  }

  close() {
    this.ref.close();
  }

  submit() {
    this.ref.close(this.jobRunnerEntity);
  }
}
