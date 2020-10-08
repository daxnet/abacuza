import { Injectable } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { CommonDialogResult } from 'app/models/common-dialog-result';
import { CommonDialogType } from 'app/models/common-dialog-type';
import { Observable, Subject } from 'rxjs';
import { CommonDialogComponent } from './common-dialog/common-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class CommonDialogService {

  constructor(private dialogService: NbDialogService) { }

  information(title: string, message: string): Observable<CommonDialogResult> {
    const subject = new Subject<CommonDialogResult>();
    this.dialogService.open(CommonDialogComponent, {
      context: {
        title,
        message,
        type: CommonDialogType.Information,
      },
      closeOnBackdropClick: false,
    })
    .onClose
    .subscribe(dr => subject.next(dr));

    return subject.asObservable();
  }

  confirm(title: string, message: string): Observable<CommonDialogResult> {
    const subject = new Subject<CommonDialogResult>();
    this.dialogService.open(CommonDialogComponent, {
      context: {
        title,
        message,
        type: CommonDialogType.Confirm,
      },
      closeOnBackdropClick: false,
    })
    .onClose
    .subscribe(dr => subject.next(dr));

    return subject.asObservable();
  }
}
