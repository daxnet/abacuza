import { Injectable } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { TextMessageDialogComponent } from './text-message-dialog/text-message-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class TextMessageDialogService {

  constructor(private dialogService: NbDialogService) { }

  show(title: string, message: string, style: string = 'font-family: \'Courier New\', Courier, monospace; font-size: x-small;'): void {
    this.dialogService.open(TextMessageDialogComponent, {
      context: {
        title: title,
        message: message,
        style: style,
      }
    });
  }
}
