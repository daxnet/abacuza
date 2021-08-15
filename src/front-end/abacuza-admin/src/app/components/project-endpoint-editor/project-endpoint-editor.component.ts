import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ComponentFactoryResolver, OnDestroy, AfterViewInit, OnChanges, SimpleChanges } from '@angular/core';
import { Endpoint } from 'src/app/models/endpoint';
import { EndpointSettingsChangedEvent } from './endpoint-settings-changed-event';
import { EndpointEditorHostDirective } from './endpoint-editor-host.directive';
import { Subscription } from 'rxjs';
import { UIComponentProviderService } from '../ui-components/uicomponent-provider.service';
import { UIComponentBase } from '../ui-components/uicomponent-base';
import { ProjectEndpointDefinition } from 'src/app/models/project-endpoint-definition';
import { EndpointsService } from 'src/app/services/endpoints.service';
import { Project } from 'src/app/models/project';

@Component({
  selector: 'app-project-endpoint-editor',
  templateUrl: './project-endpoint-editor.component.html',
  styleUrls: ['./project-endpoint-editor.component.scss']
})
export class ProjectEndpointEditorComponent implements OnInit, OnDestroy, OnChanges {
  
  @Input() project?: Project;
  @Input() endpointDefinition?: ProjectEndpointDefinition;
  @Input() endpointType: string = 'input';
  @Output() settingsChange = new EventEmitter<EndpointSettingsChangedEvent>();

  @ViewChild(EndpointEditorHostDirective, { static: true })
  endpointEditorHost?: EndpointEditorHostDirective;

  private componentEventSubscriptions: Subscription[] = [];
 
  constructor(private componentsProvider: UIComponentProviderService,
    private componentFactoryResolver: ComponentFactoryResolver,
    private endpointsService: EndpointsService) { }

  ngOnInit(): void {
      this.reloadComponents();
  }

  ngOnDestroy(): void {
    this.componentEventSubscriptions.forEach(s => s.unsubscribe());
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.reloadComponents();
  }

  private reloadComponents(): void {
    this.endpointsService.getEndpoints(this.endpointType)
        .subscribe(response => {
          const endpoints = response.body;
          if (endpoints) {
            const selectedEndpoint = endpoints.find(e => e.name === this.endpointDefinition?.name);
            if (selectedEndpoint && this.endpointDefinition && this.endpointEditorHost) {
              this.endpointDefinition.settingsObject = this.endpointDefinition?.settings ? JSON.parse(this.endpointDefinition.settings) : [];
              this.loadEndpointUIComponents(this.endpointDefinition?.id, selectedEndpoint, this.endpointEditorHost, this.endpointDefinition.settingsObject);
            }
          }
        });
  }

  private loadEndpointUIComponents(endpointDefinitionId: string, 
    endpoint: Endpoint, 
    directive: EndpointEditorHostDirective, 
    endpointSettingsObject: any): void {
    const viewContainerRef = directive.viewContainerRef;
    viewContainerRef.clear();
    endpoint.configurationUIElements.forEach(e => {
      const name = e['_type'];
      const id = endpointDefinitionId + '.' + e['_endpoint'] + '.' + e['name'];
      const item = this.componentsProvider.getRegisteredUIComponents().find(x => x.name === name);
      if (item) {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(item.component);
        const componentRef = viewContainerRef.createComponent<UIComponentBase>(componentFactory);
        componentRef.instance.id = id;
        componentRef.instance.attributes = e;
        if (this.project) {
          componentRef.instance.attributes.projectId = this.project.id;
        }
        
        let componentDataAssigned = false;
        if (endpointSettingsObject && endpointSettingsObject.length > 0) {
          const uiComponentData = endpointSettingsObject.find((o: { component: string; }) => o.component === id);
          if (uiComponentData) {
            componentRef.instance.value = uiComponentData.value;
            componentDataAssigned = true;
          }
        } 
        
        if (componentDataAssigned === false && e['defaultValueObject']){
          componentRef.instance.value = e['defaultValueObject'];
        }

        this.componentEventSubscriptions.push(componentRef.instance.modelChange.subscribe(event => {
          this.settingsChange.emit(new EndpointSettingsChangedEvent(endpointDefinitionId, endpoint, event.component, event.data));
        }));
      }
    });
  }
}
