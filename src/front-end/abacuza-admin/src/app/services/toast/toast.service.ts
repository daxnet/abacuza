import { Injectable, TemplateRef } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  toasts: any[] = [];

  show(textOrTemplate: string | TemplateRef<any>, title: string, options: any = {}) {
    this.toasts.push({ textOrTemplate, title, ...options });
  }

  remove(toast: any) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }

  info(message: string) {
    this.show(message, 'Information', { classname: 'bg-primary text-light', delay: 5000 });
  }

  warn(message: string) {
    this.show(message, 'Warning', { classname: 'bg-warning text-light', delay: 5000 });
  }

  success(message: string) {
    this.show(message, 'Success', { classname: 'bg-success text-light', delay: 5000 });
  }

  error(message: string) {
    this.show(message, 'Error', { classname: 'bg-danger text-light', delay: 5000 });
  }
}
