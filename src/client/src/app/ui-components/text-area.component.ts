import { Component, Input, OnInit } from '@angular/core';
import { UIComponentBase } from './uicomponent-base';

@Component({
  selector: 'ngx-text-area',
  templateUrl: './text-area.component.html',
  styleUrls: ['./text-area.component.scss']
})
export class TextAreaComponent implements UIComponentBase, OnInit {

  @Input() attributes: any;
  
  constructor() { }

  ngOnInit(): void {
  }

}
