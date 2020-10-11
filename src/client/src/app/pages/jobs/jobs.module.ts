import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobRunnersComponent } from './job-runners/job-runners.component';
import { JobsComponent } from './jobs.component';
import { JobsRoutingModule } from './jobs-routing.module';
import { NbCardModule, NbToastrModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';



@NgModule({
  declarations: [JobsComponent, JobRunnersComponent],
  imports: [
    CommonModule,
    NbCardModule,
    JobsRoutingModule,
    NbToastrModule.forRoot(),
    Ng2SmartTableModule,
  ],
})
export class JobsModule { }
