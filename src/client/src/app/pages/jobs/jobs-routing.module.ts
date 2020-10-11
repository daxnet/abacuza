import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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
