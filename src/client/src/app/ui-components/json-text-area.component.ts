import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { JsonEditorOptions } from 'ang-jsoneditor';
import { ComponentEvent } from './component-event';
import { UIComponentBase } from './uicomponent-base';

@Component({
  selector: 'ngx-json-text-area',
  templateUrl: './json-text-area.component.html',
  styleUrls: ['./json-text-area.component.scss']
})
export class JsonTextAreaComponent implements UIComponentBase, OnInit {

  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string;
  @Output() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  editorOptions: JsonEditorOptions;

  constructor() { 
    this.editorOptions = new JsonEditorOptions();
    this.editorOptions.mode = 'code';
    this.editorOptions.mainMenuBar = false;
  }
  

  ngOnInit(): void {
  }

  func(event): void {
    console.log(this.value);
    this.modelChange.emit(new ComponentEvent(this.id, this.value));
  }
}
