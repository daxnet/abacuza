import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectListComponent } from './project-list/project-list.component';
import { ProjectsComponent } from './projects.component';
import { FormsModule } from '@angular/forms';
import { NbButtonModule, NbCardModule, NbInputModule, NbSelectModule, NbTabsetModule, NbToastrModule } from '@nebular/theme';
import { JobsRoutingModule } from '../jobs/jobs-routing.module';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ProjectsRoutingModule } from './projects-routing.module';
import { CreateProjectComponent } from './project-list/create-project/create-project.component';
import { ProjectDetailsComponent } from './project-list/project-details/project-details.component';
import { UIComponentsHostDirective } from 'app/ui-components/uicomponents-host.directive';

@NgModule({
  declarations: [
    ProjectsComponent, 
    ProjectListComponent, 
    CreateProjectComponent, 
    ProjectDetailsComponent,
    UIComponentsHostDirective,],
  imports: [
    FormsModule,
    CommonModule,
    NbButtonModule,
    NbCardModule,
    NbSelectModule,
    NbInputModule,
    NbTabsetModule,
    JobsRoutingModule,
    ProjectsRoutingModule,
    NbToastrModule.forRoot({
      duration: 6000,
    }),
    Ng2SmartTableModule,
  ]
})
export class ProjectsModule { }
