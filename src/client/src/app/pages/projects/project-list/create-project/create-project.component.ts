import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { Endpoint } from 'app/models/endpoint';
import { JobRunner } from 'app/models/job-runner';
import { Project } from 'app/models/project';

@Component({
  selector: 'ngx-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss']
})
export class CreateProjectComponent implements OnInit {

  @Input() inputEndpoints: Endpoint[];
  @Input() jobRunners: JobRunner[];
  @Input() projectEntity: Project;
  
  constructor(protected ref: NbDialogRef<CreateProjectComponent>) { }

  ngOnInit(): void {
  }

  submit(): void {
    this.ref.close(this.projectEntity);
  }

  close(): void {
    this.ref.close();
  }
}
