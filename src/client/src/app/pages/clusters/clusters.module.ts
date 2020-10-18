import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  NbCardModule,
  NbIconModule,
  NbDialogModule,
  NbButtonModule,
  NbSelectModule,
  NbInputModule, NbToastrModule } from '@nebular/theme';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { CommonModule } from '@angular/common';
import { ClustersComponent } from './clusters.component';
import { ClusterTypesComponent } from './cluster-types/cluster-types.component';
import { ClustersRoutingModule } from './clusters-routing.module';
import { ClusterConnectionsComponent } from './cluster-connections/cluster-connections.component';
import { EditClusterConnectionComponent } from './cluster-connections/edit-cluster-connection/edit-cluster-connection.component';

import { ThemeModule } from '../../@theme/theme.module';

@NgModule({
  declarations: [
    ClustersComponent,
    ClusterTypesComponent,
    ClusterConnectionsComponent,
    EditClusterConnectionComponent,
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
    NbToastrModule.forRoot({
      duration: 6000,
    }),
    NbDialogModule.forRoot(),
    ClustersRoutingModule,
    Ng2SmartTableModule,
  ],
  entryComponents: [
    EditClusterConnectionComponent,
  ],
})
export class ClustersModule { }
