import { NgModule } from '@angular/core';
import { NbCardModule, NbIconModule } from '@nebular/theme';
import { CommonModule } from '@angular/common';
import { ClustersComponent } from './clusters.component';
import { ClusterTypesComponent } from './cluster-types/cluster-types.component';
import { ClustersRoutingModule } from './clusters-routing.module';

@NgModule({
  declarations: [ClustersComponent, ClusterTypesComponent],
  imports: [
    CommonModule,
    NbCardModule,
    NbIconModule,
    ClustersRoutingModule,
  ],
})
export class ClustersModule { }
