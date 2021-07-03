import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appComponentDialogContent]'
})
export class ComponentDialogContentDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
