import { Component, Input, OnInit } from '@angular/core';
import { ComponentDialogBase } from 'src/app/services/component-dialog/component-dialog-base';
import { ComponentDialogUsage } from 'src/app/services/component-dialog/component-dialog-options';

@Component({
  selector: 'app-create-job-runner',
  templateUrl: './create-job-runner.component.html',
  styleUrls: ['./create-job-runner.component.scss']
})
export class CreateJobRunnerComponent implements OnInit, ComponentDialogBase {

  @Input() usage?: ComponentDialogUsage | undefined;
  @Input() data: any;

  constructor() { }
  

  ngOnInit(): void {
  }

}
