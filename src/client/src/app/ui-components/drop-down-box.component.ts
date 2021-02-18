import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ComponentEvent } from './component-event';
import { UIComponentBase } from './uicomponent-base';

@Component({
  selector: 'ngx-drop-down-box',
  templateUrl: './drop-down-box.component.html',
  styleUrls: ['./drop-down-box.component.scss'],
})
export class DropDownBoxComponent implements UIComponentBase, OnInit {

  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string;
  @Output() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  constructor() { }

  ngOnInit(): void {
    this.attributes.options = this.attributes.options.split(',');
  }

  onSelectionChanged(event): void {
    this.modelChange.emit(new ComponentEvent(this.id, event));
  }
}
