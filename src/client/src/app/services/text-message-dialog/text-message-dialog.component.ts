import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'ngx-text-message-dialog',
  templateUrl: './text-message-dialog.component.html',
  styleUrls: ['./text-message-dialog.component.scss']
})
export class TextMessageDialogComponent implements OnInit {

  @Input() title: string;
  @Input() message: string;
  @Input() style: string;
  
  constructor(protected ref: NbDialogRef<TextMessageDialogComponent>) { }

  ngOnInit(): void {
  }

  ok(): void {
    this.ref.close();
  }
}
