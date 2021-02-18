import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ComponentEvent } from './component-event';
import { UIComponentBase } from './uicomponent-base';

@Component({
  selector: 'ngx-file-picker',
  templateUrl: './file-picker.component.html',
  styleUrls: ['./file-picker.component.scss']
})
export class FilePickerComponent implements UIComponentBase, OnInit {

  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string;
  @Output() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  s3Key: string;

  constructor() { }

  ngOnInit(): void {
    this.s3Key = `projects/${this.attributes.contextualEntityId}/files`;
  }

  onFilesChangedCompleted(event: any): void {
    this.modelChange.emit(new ComponentEvent(this.id, event));
  }
}
