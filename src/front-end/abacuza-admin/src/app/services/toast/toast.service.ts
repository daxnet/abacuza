import { Injectable, TemplateRef } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  toasts: any[] = [];

  show(textOrTemplate: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTemplate, ...options });
  }

  remove(toast: any) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }

  info(message: string) {
    this.show(message, { classname: 'bg-primary text-light', delay: 5000 });
  }

  warn(message: string) {
    this.show(message, { classname: 'bg-warning text-light', delay: 5000 });
  }

  success(message: string) {
    this.show(message, { classname: 'bg-success text-light', delay: 5000 });
  }

  error(message: string) {
    this.show(message, { classname: 'bg-danger text-light', delay: 5000 });
  }
}
