import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobRunnerDetailsComponent } from './job-runners/job-runner-details/job-runner-details.component';
import { JobRunnersComponent } from './job-runners/job-runners.component';
import { JobsComponent } from './jobs.component';

const routes: Routes = [
    {
        path: '',
        component: JobsComponent,
        children: [
            {
                path: 'job-runners',
                component: JobRunnersComponent,
            },
            {
                path: 'job-runner-details/:id',
                component: JobRunnerDetailsComponent,
            },
        ],
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ],
    exports: [
        RouterModule,
    ],
})
export class JobsRoutingModule {
}
