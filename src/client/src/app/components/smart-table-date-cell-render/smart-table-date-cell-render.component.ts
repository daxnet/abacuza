import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';

@Component({
  selector: 'ngx-smart-table-date-cell-render',
  templateUrl: './smart-table-date-cell-render.component.html',
  styleUrls: ['./smart-table-date-cell-render.component.scss'],
})
export class SmartTableDateCellRenderComponent implements ViewCell, OnInit {
  renderValue: string;

  constructor() { }

  @Input() value: string | number;
  @Input() rowData: any;

  ngOnInit(): void {
    const utcDateValue = Date.parse(this.value.toString());
    const date = new Date(utcDateValue);
    this.renderValue = `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
  }
}
