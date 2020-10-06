import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ClustersComponent } from './clusters.component';
import { ClusterTypesComponent } from './cluster-types/cluster-types.component';
import { ClusterConnectionsComponent } from './cluster-connections/cluster-connections.component';

const routes: Routes = [
    {
        path: '',
        component: ClustersComponent,
        children: [
            {
                path: 'types',
                component: ClusterTypesComponent,
            },
            {
                path: 'connections',
                component: ClusterConnectionsComponent,
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
export class ClustersRoutingModule {
}
