import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[ngxUIComponentsInputEndpointHost]',
})
export class UIComponentsInputEndpointHostDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
