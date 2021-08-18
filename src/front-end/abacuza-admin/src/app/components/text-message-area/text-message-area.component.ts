import { Component, Input, OnInit } from '@angular/core';
import { ComponentDialogBase } from 'src/app/services/component-dialog/component-dialog-base';
import { ComponentDialogUsage } from 'src/app/services/component-dialog/component-dialog-options';

@Component({
  selector: 'app-text-message-area',
  templateUrl: './text-message-area.component.html',
  styleUrls: ['./text-message-area.component.scss']
})
export class TextMessageAreaComponent implements OnInit, ComponentDialogBase {

  @Input() usage?: ComponentDialogUsage | undefined;
  @Input() data: any;

  constructor() { }
  
  ngOnInit(): void {
  }

}
