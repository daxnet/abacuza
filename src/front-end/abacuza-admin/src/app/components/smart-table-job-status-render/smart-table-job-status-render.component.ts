import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-smart-table-job-status-render',
  templateUrl: './smart-table-job-status-render.component.html',
  styleUrls: ['./smart-table-job-status-render.component.scss']
})
export class SmartTableJobStatusRenderComponent implements OnInit {

  renderValue: string = '';
  renderClass: string = '';
  dateTextClass: string = '';
  dateText: string = '';

  constructor() { }

  @Input() value: string | number = '';
  @Input() rowData: any;

  ngOnInit(): void {
    if (!this.value) {
      this.renderValue = '';
    } else {
      switch (this.value) {
        case 'Created':
          this.renderClass = 'badge badge-light';
          break;
        case 'Completed':
          this.renderClass = 'badge badge-success';
          this.dateText = this.getDateText(this.rowData.jobCompletedDate);
          this.dateTextClass = 'small text-success';
          break;
        case 'Failed':
          this.renderClass = 'badge badge-danger';
          this.dateText = this.getDateText(this.rowData.jobFailedDate);
          this.dateTextClass = 'small text-danger';
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

  getDateText(date: any): string {
    return new Date(date).toLocaleString();
  }

}
