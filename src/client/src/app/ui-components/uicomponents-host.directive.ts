import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[ngxUIComponentsHost]'
})
export class UIComponentsHostDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
