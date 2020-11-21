import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectListComponent } from './project-list/project-list.component';
import { ProjectsComponent } from './projects.component';
import { FormsModule } from '@angular/forms';
import { NbButtonModule, NbCardModule, NbCheckboxModule, NbIconModule, NbInputModule, NbProgressBarModule, NbSelectModule, NbTabsetModule, NbToastrModule, NbTooltipModule } from '@nebular/theme';
import { JobsRoutingModule } from '../jobs/jobs-routing.module';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ProjectsRoutingModule } from './projects-routing.module';
import { CreateProjectComponent } from './project-list/create-project/create-project.component';
import { ProjectDetailsComponent } from './project-list/project-details/project-details.component';
import { UIComponentsHostDirective } from 'app/ui-components/uicomponents-host.directive';
import { FileListComponent } from 'app/components/file-list/file-list.component';
import { SharedModule } from 'app/shared/shared.module';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { ThemeModule } from 'app/@theme/theme.module';

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
    ThemeModule,
    JobsRoutingModule,
    Ng2SmartTableModule,
    NbEvaIconsModule,
    NbIconModule,
    NbInputModule,
    NbCardModule,
    NbButtonModule,
    NbCheckboxModule,
    NbSelectModule,
    NbToastrModule,
    NbTooltipModule,
    NbProgressBarModule,
    NbTabsetModule,
    ProjectsRoutingModule,
    NbToastrModule.forRoot({
      duration: 6000,
    }),
    SharedModule,
  ],
})
export class ProjectsModule { }
