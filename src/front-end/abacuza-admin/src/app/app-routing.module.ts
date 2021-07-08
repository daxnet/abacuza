import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { ClusterConnectionsComponent } from './pages/cluster-connections/cluster-connections.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { InstalledPluginsComponent } from './pages/installed-plugins/installed-plugins.component';
import { JobRunnerDetailsComponent } from './pages/job-runner-details/job-runner-details.component';
import { JobRunnersComponent } from './pages/job-runners/job-runners.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ProjectsComponent } from './pages/projects/projects.component';
import { AuthGuardService } from './services/auth-guard.service';

const routes: Routes = [
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: 'cluster-connections', component: ClusterConnectionsComponent, canActivate: [AuthGuardService] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService] },
  { path: 'installed-plugins', component: InstalledPluginsComponent, canActivate: [AuthGuardService] },
  { path: 'job-runners', component: JobRunnersComponent, canActivate: [AuthGuardService] },
  { path: 'job-runners/details/:id', component: JobRunnerDetailsComponent, canActivate: [AuthGuardService] },
  { path: 'projects', component: ProjectsComponent, canActivate: [AuthGuardService] },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: '**', component: DashboardComponent, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
