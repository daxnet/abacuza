import { Component, OnInit, EventEmitter, Input } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';
import { ComponentEvent } from './component-event';

@Component({
  selector: 'app-text-box',
  templateUrl: './text-box.component.html',
  styleUrls: ['./text-box.component.scss']
})
export class TextBoxComponent implements UIComponentBase, OnInit {

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
