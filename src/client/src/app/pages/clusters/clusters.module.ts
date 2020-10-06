import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  NbCardModule,
  NbIconModule,
  NbDialogModule,
  NbButtonModule,
  NbSelectModule,
  NbInputModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { CommonModule } from '@angular/common';
import { ClustersComponent } from './clusters.component';
import { ClusterTypesComponent } from './cluster-types/cluster-types.component';
import { ClustersRoutingModule } from './clusters-routing.module';
import { ClusterConnectionsComponent } from './cluster-connections/cluster-connections.component';
import { CreateClusterConnectionComponent } from './cluster-connections/create-cluster-connection/create-cluster-connection.component';

import { ThemeModule } from '../../@theme/theme.module';

@NgModule({
  declarations: [
    ClustersComponent,
    ClusterTypesComponent,
    ClusterConnectionsComponent,
    CreateClusterConnectionComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ThemeModule,
    NbButtonModule,
    NbSelectModule,
    NbCardModule,
    NbIconModule,
    NbInputModule,
    NbDialogModule.forRoot(),
    ClustersRoutingModule,
    Ng2SmartTableModule,
  ],
  entryComponents: [
    CreateClusterConnectionComponent,
  ],
})
export class ClustersModule { }
