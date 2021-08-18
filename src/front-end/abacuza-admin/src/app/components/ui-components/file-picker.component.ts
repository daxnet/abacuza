import { Component, OnInit, EventEmitter, Input } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';
import { ComponentEvent } from './component-event';

@Component({
  selector: 'app-file-picker',
  templateUrl: './file-picker.component.html',
  styleUrls: ['./file-picker.component.scss']
})
export class FilePickerComponent implements UIComponentBase, OnInit {
  
  @Input() attributes: any;
  @Input() value: any;
  @Input() id: string = '';
  @Input() modelChange: EventEmitter<ComponentEvent> = new EventEmitter<ComponentEvent>();

  s3Key: string = '';

  constructor() { }

  ngOnInit(): void {
    this.s3Key = `projects/${this.attributes.projectId}/files`;
  }

  onFilesChangedCompleted(evnt: any) {
    this.modelChange.emit(new ComponentEvent(this.id, evnt));
  }

}
