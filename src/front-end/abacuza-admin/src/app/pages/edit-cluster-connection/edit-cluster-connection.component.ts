import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { JsonEditorOptions } from 'ang-jsoneditor';
import { ComponentDialogBase } from 'src/app/services/component-dialog/component-dialog-base';
import { ComponentDialogEvent } from 'src/app/services/component-dialog/component-dialog-event';
import { ComponentDialogUsage } from 'src/app/services/component-dialog/component-dialog-options';

@Component({
  selector: 'app-edit-cluster-connection',
  templateUrl: './edit-cluster-connection.component.html',
  styleUrls: ['./edit-cluster-connection.component.scss']
})
export class EditClusterConnectionComponent implements OnInit, ComponentDialogBase {

  @Input() data: any;
  @Input() usage?: ComponentDialogUsage;

  jsonEditorOptions: JsonEditorOptions = new JsonEditorOptions();

  constructor() {
    this.jsonEditorOptions.mode = 'code';
    this.jsonEditorOptions.mainMenuBar = false;
  }
  
  ngOnInit(): void {
  }

}
