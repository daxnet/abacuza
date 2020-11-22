import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { PagesComponent } from './pages.component';
import { NotFoundComponent } from './miscellaneous/not-found/not-found.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'projects',
      loadChildren: () => import('./projects/projects.module')
        .then(m => m.ProjectsModule),
    },
    {
      path: 'clusters',
      loadChildren: () => import('./clusters/clusters.module')
        .then(m => m.ClustersModule),
    },
    {
      path: 'jobs',
      loadChildren: () => import('./jobs/jobs.module')
        .then(m => m.JobsModule),
    },
    {
      path: '',
      redirectTo: 'projects/project-list',
      pathMatch: 'full',
    },
    {
      path: '**',
      component: NotFoundComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
