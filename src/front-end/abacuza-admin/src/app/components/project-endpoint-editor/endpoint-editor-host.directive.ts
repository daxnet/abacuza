import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appEndpointEditorHost]'
})
export class EndpointEditorHostDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
