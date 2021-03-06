import { NgModule } from '@angular/core';
import { NbMenuModule, NbTabsetModule } from '@nebular/theme';

import { ThemeModule } from '../@theme/theme.module';
import { PagesComponent } from './pages.component';
import { PagesRoutingModule } from './pages-routing.module';
import { MiscellaneousModule } from './miscellaneous/miscellaneous.module';

import { ClustersService } from '../services/clusters.service';
import { ClusterConnectionsService } from 'app/services/cluster-connections.service';
import { JobRunnersService } from 'app/services/job-runners.service';
import { CommonService } from 'app/services/common.service';
import { ProjectsService } from 'app/services/projects.service';
import { EndpointsService } from 'app/services/endpoints.service';

@NgModule({
  imports: [
    PagesRoutingModule,
    ThemeModule,
    NbMenuModule,
    MiscellaneousModule,
  ],
  declarations: [
    PagesComponent,
  ],
  providers: [
    ClustersService,
    ClusterConnectionsService,
    JobRunnersService,
    ProjectsService,
    EndpointsService,
    CommonService,
  ],
})
export class PagesModule {
}
