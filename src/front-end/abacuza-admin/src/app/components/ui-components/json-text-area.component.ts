import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';
import { ComponentEvent } from './component-event';
import { JsonEditorOptions } from 'ang-jsoneditor';

@Component({
  selector: 'app-json-text-area',
  templateUrl: './json-text-area.component.html',
  styleUrls: ['./json-text-area.component.scss']
})
export class JsonTextAreaComponent implements UIComponentBase, OnInit {
  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string = '';
  @Input() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  editorOptions: JsonEditorOptions = new JsonEditorOptions();

  constructor() {
    this.editorOptions.mode = 'code';
    this.editorOptions.mainMenuBar = false;
  }

  ngOnInit(): void {
  }

  change(event: any): void {
    this.modelChange.emit(new ComponentEvent(this.id, this.value));
  }
}
