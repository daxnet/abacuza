import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'ngx-smart-table-job-status-render',
  templateUrl: './smart-table-job-status-render.component.html',
  styleUrls: ['./smart-table-job-status-render.component.scss'],
})
export class SmartTableJobStatusRenderComponent implements OnInit {

  renderValue: string;
  renderClass: string;

  constructor() { }

  @Input() value: string | number;
  @Input() rowData: any;

  ngOnInit(): void {
    if (!this.value) {
      this.renderValue = '';
    } else {
      switch (this.value) {
        case 'Completed':
          this.renderClass = 'badge badge-success';
          break;
        case 'Failed':
          this.renderClass = 'badge badge-danger';
          break;
        case 'Running':
          this.renderClass = 'badge badge-info';
          break;
        default:
          this.renderClass = 'badge badge-light';
          break;
      }

      this.renderValue = this.value.toString().toUpperCase();
    }
  }
}
