import { Component, OnInit, Input } from '@angular/core';
import { Endpoint } from 'src/app/models/endpoint';
import { ProjectEndpointDefinition } from 'src/app/models/project-endpoint-definition';

@Component({
  selector: 'app-project-endpoint-editor',
  templateUrl: './project-endpoint-editor.component.html',
  styleUrls: ['./project-endpoint-editor.component.scss']
})
export class ProjectEndpointEditorComponent implements OnInit {

  @Input() title: string = 'Endpoints';
  @Input() endpoints: Endpoint[] | null = [];
  @Input() definitions: ProjectEndpointDefinition[] | null = [];
  
  constructor() { }

  ngOnInit(): void {
  }

}
