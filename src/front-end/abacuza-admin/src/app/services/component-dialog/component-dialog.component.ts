import { Component, ComponentFactoryResolver, Input, OnInit, Type, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComponentDialogBase } from './component-dialog-base';
import { ComponentDialogContentDirective } from './component-dialog-content.directive';
import { ComponentDialogOptions } from './component-dialog-options';

@Component({
  selector: 'app-component-dialog',
  templateUrl: './component-dialog.component.html',
  styleUrls: ['./component-dialog.component.scss']
})
export class ComponentDialogComponent implements OnInit {

  @ViewChild(ComponentDialogContentDirective, {static: true})
  appComponentDialogContent?: ComponentDialogContentDirective;

  @Input() component?: Type<any>;
  @Input() data: any;
  @Input() options?: ComponentDialogOptions;
  
  constructor(public activeModal: NgbActiveModal, private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
    const viewContainerRef = this.appComponentDialogContent?.viewContainerRef;
    if (viewContainerRef) {
      viewContainerRef.clear();
      const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.component!);
      const componentRef = viewContainerRef.createComponent<ComponentDialogBase>(componentFactory);
      componentRef.instance.data = this.data;
      componentRef.instance.usage = this.options?.usage;
    }
  }

}
