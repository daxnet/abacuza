import { Component, Input, OnInit } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';

@Component({
  selector: 'app-smart-table-date-cell-render',
  templateUrl: './smart-table-date-cell-render.component.html',
  styleUrls: ['./smart-table-date-cell-render.component.scss']
})
export class SmartTableDateCellRenderComponent implements OnInit, ViewCell {

  renderValue: string = '';

  @Input() value: string | number = '';
  @Input() rowData: any;

  constructor() { }

  ngOnInit(): void {
    if (!this.value) {
      this.renderValue = '';
    } else {
      const utcDateValue = Date.parse(this.value.toString());
      const date = new Date(utcDateValue);
      this.renderValue = date.toLocaleString();
    }
  }

}
