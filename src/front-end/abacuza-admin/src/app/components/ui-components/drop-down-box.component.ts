import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';
import { ComponentEvent } from './component-event';

@Component({
  selector: 'app-drop-down-box',
  templateUrl: './drop-down-box.component.html',
  styleUrls: ['./drop-down-box.component.scss']
})
export class DropDownBoxComponent implements UIComponentBase, OnInit {
  
  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string = '';
  @Input() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  constructor() { }

  ngOnInit(): void {
    this.attributes.options = this.attributes.options.split(',');
  }

  onSelectionChanged(event: any): void {
    this.modelChange.emit(new ComponentEvent(this.id, event.target.value));
  }
}
