import { Component, Input, OnInit } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';

@Component({
  selector: 'ngx-check-box',
  templateUrl: './check-box.component.html',
  styleUrls: ['./check-box.component.scss']
})
export class CheckBoxComponent implements UIComponentBase, OnInit {

  @Input() attributes: any;

  constructor() { }

  ngOnInit(): void {
  }

}
