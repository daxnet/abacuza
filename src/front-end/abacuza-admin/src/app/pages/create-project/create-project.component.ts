import { Component, OnInit, Input } from '@angular/core';
import { ComponentDialogBase } from 'src/app/services/component-dialog/component-dialog-base';
import { ComponentDialogUsage } from 'src/app/services/component-dialog/component-dialog-options';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss']
})
export class CreateProjectComponent implements OnInit, ComponentDialogBase {
  
  @Input() usage?: ComponentDialogUsage | undefined;
  @Input() data: any;

  constructor() { }

  ngOnInit(): void {
  }

}
