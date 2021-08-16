import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';
import { ComponentEvent } from './component-event';

@Component({
  selector: 'app-text-area',
  templateUrl: './text-area.component.html',
  styleUrls: ['./text-area.component.scss']
})
export class TextAreaComponent implements UIComponentBase, OnInit {
  
  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string = '';
  @Input() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  constructor() { }

  ngOnInit(): void {
  }

  onBlur(event: any): void {
    this.modelChange.emit(new ComponentEvent(this.id, event.target.value));
  }
}
