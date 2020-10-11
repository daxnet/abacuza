import { NgModule } from '@angular/core';
import { NbMenuModule } from '@nebular/theme';

import { ThemeModule } from '../@theme/theme.module';
import { PagesComponent } from './pages.component';
import { PagesRoutingModule } from './pages-routing.module';
import { MiscellaneousModule } from './miscellaneous/miscellaneous.module';

import { ClustersService } from '../services/clusters.service';
import { ClusterConnectionsService } from 'app/services/cluster-connections.service';
import { JobRunnersService } from 'app/services/job-runners.service';

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
  ],
})
export class PagesModule {
}
