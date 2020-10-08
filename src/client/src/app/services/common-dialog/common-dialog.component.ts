import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { CommonDialogResult } from 'app/models/common-dialog-result';
import { CommonDialogType } from 'app/models/common-dialog-type';

@Component({
  selector: 'ngx-common-dialog',
  templateUrl: './common-dialog.component.html',
  styleUrls: ['./common-dialog.component.scss'],
})
export class CommonDialogComponent implements OnInit {

  @Input() type: CommonDialogType;
  @Input() title: string;
  @Input() message: string;

  iconName: string;
  iconStatus: string;
  dialogResult: CommonDialogResult;

  constructor(protected ref: NbDialogRef<CommonDialogComponent>) { }

  ngOnInit(): void {
    switch (this.type) {
      case CommonDialogType.Information:
        this.iconName = 'info';
        this.iconStatus = 'info';
        break;
      case CommonDialogType.Confirm:
        this.iconName = 'alert-triangle';
        this.iconStatus = 'warning';
        break;
    }
  }

  ok() {
    this.ref.close(CommonDialogResult.Ok);
  }

  yes() {
    this.ref.close(CommonDialogResult.Yes);
  }

  no() {
    this.ref.close(CommonDialogResult.No);
  }
}
