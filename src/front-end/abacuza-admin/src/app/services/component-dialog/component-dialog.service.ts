import { Injectable, Type } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { ComponentDialogComponent } from './component-dialog.component';
import { cloneDeep } from 'lodash';
import { ComponentDialogOptions } from './component-dialog-options';

@Injectable({
  providedIn: 'root'
})
export class ComponentDialogService {

  constructor(private modalService: NgbModal) { }

  open(component: Type<any>, 
    data: any = undefined, 
    options: ComponentDialogOptions | undefined = undefined,
    size: 'lg' | 'xl' | 'sm' | undefined = undefined): Observable<any> {
    let dlgOpts: any = {
      backdrop: 'static',
      keyboard: false
    };
    if (size) {
      dlgOpts.size = size;
    }

    const modalRef = this.modalService.open(ComponentDialogComponent, dlgOpts);
    modalRef.componentInstance.component = component;
    const defaultOption = ComponentDialogOptions.createDefault();
    if (!options) {
      options = defaultOption;
    } else {
      if (!options.acceptButtonText) {
        options.acceptButtonText = defaultOption.acceptButtonText;
      }
      if (!options.cancelButtonText) {
        options.cancelButtonText = defaultOption.cancelButtonText;
      }
    }
    modalRef.componentInstance.options = options;
    if (data) {
      modalRef.componentInstance.data = cloneDeep(data);
    }
    return modalRef.closed;
  }
}
