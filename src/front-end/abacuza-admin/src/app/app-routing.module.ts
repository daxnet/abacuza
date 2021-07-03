import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { ClusterConnectionsComponent } from './pages/cluster-connections/cluster-connections.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { InstalledPluginsComponent } from './pages/installed-plugins/installed-plugins.component';
import { JobRunnersComponent } from './pages/job-runners/job-runners.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { AuthGuardService } from './services/auth-guard.service';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService] },
  { path: 'installed-plugins', component: InstalledPluginsComponent, canActivate: [AuthGuardService] },
  { path: 'cluster-connections', component: ClusterConnectionsComponent, canActivate: [AuthGuardService] },
  { path: 'job-runners', component: JobRunnersComponent, canActivate: [AuthGuardService] },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: '**', component: DashboardComponent, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
