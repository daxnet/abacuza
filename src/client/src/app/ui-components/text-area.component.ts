import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ComponentEvent } from './component-event';
import { UIComponentBase } from './uicomponent-base';

@Component({
  selector: 'ngx-text-area',
  templateUrl: './text-area.component.html',
  styleUrls: ['./text-area.component.scss'],
})
export class TextAreaComponent implements UIComponentBase, OnInit {

  @Input() attributes: any;
  @Input() value: any;
  @Output() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  constructor() { }

  ngOnInit(): void {
  }

  onBlur(event): void {
    this.modelChange.emit(new ComponentEvent(this.attributes.name, event.target.value));
  }
}