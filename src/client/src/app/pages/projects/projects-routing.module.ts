import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjectsComponent } from './projects.component';
import { ProjectListComponent } from './project-list/project-list.component';
import { ProjectDetailsComponent } from './project-list/project-details/project-details.component';

const routes: Routes = [
    {
        path: '',
        component: ProjectsComponent,
        children: [
            {
                path: 'project-list',
                component: ProjectListComponent,
            },
            {
                path: 'project-details/:id',
                component: ProjectDetailsComponent,
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
export class ProjectsRoutingModule {
}
