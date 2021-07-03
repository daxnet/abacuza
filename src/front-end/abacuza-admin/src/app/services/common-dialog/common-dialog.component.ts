import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonDialogType } from './common-dialog-data-types';

@Component({
  selector: 'app-common-dialog',
  templateUrl: './common-dialog.component.html',
  styleUrls: ['./common-dialog.component.scss']
})
export class CommonDialogComponent {

  @Input() title: string | null = 'Title';
  @Input() dialogType: CommonDialogType = CommonDialogType.Information;
  @Input() message: string | null = null;

  constructor(public activeModal: NgbActiveModal) {
  }
}
