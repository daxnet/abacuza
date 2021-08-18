import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';
import { ComponentEvent } from './component-event';

@Component({
  selector: 'app-check-box',
  templateUrl: './check-box.component.html',
  styleUrls: ['./check-box.component.scss']
})
export class CheckBoxComponent implements UIComponentBase, OnInit {
  
  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string = '';
  @Input() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  constructor() { }

  ngOnInit(): void {
  }

  checkChanged(event: any): void {
    this.modelChange.emit(new ComponentEvent(this.id, event.target.checked));
  }
}
