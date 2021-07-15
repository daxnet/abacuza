import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { Endpoint } from 'src/app/models/endpoint';
import { EndpointSettingsChangedEvent } from './endpoint-settings-changed-event';
import { EndpointEditorHostDirective } from './endpoint-editor-host.directive';
import { Subscription } from 'rxjs';
import { UIComponentProviderService } from '../ui-components/uicomponent-provider.service';
import { UIComponentBase } from '../ui-components/uicomponent-base';

@Component({
  selector: 'app-project-endpoint-editor',
  templateUrl: './project-endpoint-editor.component.html',
  styleUrls: ['./project-endpoint-editor.component.scss']
})
export class ProjectEndpointEditorComponent implements OnInit {

  @Input() selectedEndpoint?: Endpoint;
  @Output() settingsChange = new EventEmitter<EndpointSettingsChangedEvent>();

  @ViewChild(EndpointEditorHostDirective, { static: true })
  endpointEditorHost?: EndpointEditorHostDirective;

  private componentEventSubscriptions: Subscription[] = [];
 
  constructor(private componentsProvider: UIComponentProviderService,
    private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
  }

  private loadEndpointUIComponents(endpoint: Endpoint, directive: EndpointEditorHostDirective): void {
    const viewContainerRef = directive.viewContainerRef;
    viewContainerRef.clear();
    endpoint.configurationUIElements.forEach(e => {
      const name = e['_type'];
      const id = e['_endpoint'] + '.' + e['name'];
      const item = this.componentsProvider.getRegisteredUIComponents().find(x => x.name === name);
      if (item) {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
        const componentRef = viewContainerRef.createComponent<UIComponentBase>(componentFactory);
        componentRef.instance.attributes = e;
        componentRef.instance.id = id;
        this.componentEventSubscriptions.push(componentRef.instance.modelChange.subscribe(event => {
          this.settingsChange.emit(new EndpointSettingsChangedEvent(endpoint, event.component, event.data));
        }));
      }
    })
  }
}
