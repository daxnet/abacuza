import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ComponentEvent } from './component-event';
import { UIComponentBase } from './uicomponent-base';

@Component({
  selector: 'ngx-text-box',
  templateUrl: './text-box.component.html',
  styleUrls: ['./text-box.component.scss'],
})
export class TextBoxComponent implements UIComponentBase, OnInit {

  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string;
  @Output() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  constructor() { }

  ngOnInit(): void {
  }

  onBlur(event): void {
    this.modelChange.emit(new ComponentEvent(this.id, event.target.value));
  }
}
