import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { CommonDialogType } from './common-dialog-data-types';
import { CommonDialogComponent } from './common-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class CommonDialogService {

  constructor(private modalService: NgbModal) { }

  open(title: string, message: string, type: CommonDialogType): Observable<any> {
    const modalRef = this.modalService.open(CommonDialogComponent);
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.dialogType = type;
    return modalRef.closed;
  }
}
