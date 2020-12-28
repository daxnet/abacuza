import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[ngxUIComponentsOutputEndpointHost]',
})
export class UIComponentsOutputEndpointHostDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
