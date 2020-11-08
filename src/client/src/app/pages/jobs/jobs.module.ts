import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobRunnersComponent } from './job-runners/job-runners.component';
import { JobsComponent } from './jobs.component';
import { JobsRoutingModule } from './jobs-routing.module';
import { NbCardModule, NbToastrModule, NbSelectModule, NbInputModule, NbButtonModule, NbCheckboxModule, NbIconModule, NbProgressBarModule, NbTooltipModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { CreateJobRunnerComponent } from './job-runners/create-job-runner/create-job-runner.component';
import { JobRunnerDetailsComponent } from './job-runners/job-runner-details/job-runner-details.component';
import { FormsModule } from '@angular/forms';
import { FileListComponent } from 'app/components/file-list/file-list.component';
import { SharedModule } from 'app/shared/shared.module';
import { NbEvaIconsModule } from '@nebular/eva-icons';

@NgModule({
  declarations: [JobsComponent, JobRunnersComponent, CreateJobRunnerComponent, JobRunnerDetailsComponent],
  imports: [
    FormsModule,
    CommonModule,
    NbCardModule,
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
    JobsRoutingModule,
    SharedModule,
    NbToastrModule.forRoot({
      duration: 6000,
    }),
  ],
})
export class JobsModule { }
