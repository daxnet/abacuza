import { Component, OnDestroy, OnInit } from '@angular/core';
import { Cluster } from '../../models/cluster';
import { ClustersService } from 'src/app/services/clusters.service';
import { EndpointsService } from 'src/app/services/endpoints.service';
import { Endpoint } from 'src/app/models/endpoint';

@Component({
  selector: 'app-installed-plugins',
  templateUrl: './installed-plugins.component.html',
  styleUrls: ['./installed-plugins.component.scss']
})
export class InstalledPluginsComponent implements OnInit, OnDestroy {

  clusters: Cluster[] | null = null;
  endpoints: Endpoint[] | null = null;

  dtSupportedClustersOptions: DataTables.Settings = {};
  dtSupportedEndpointsOptions: DataTables.Settings = {};

  constructor(private clustersService: ClustersService, private endpointsService: EndpointsService) {
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
    this.dtSupportedClustersOptions = {
      pagingType: 'simple'
    };
    this.dtSupportedEndpointsOptions = {
      pagingType: 'simple'
    };

    this.clustersService.getClusters()
      .subscribe(response => {
        this.clusters = response.body;
      });

    this.endpointsService.getEndpoints('all')
      .subscribe(response => {
        this.endpoints = response.body;
      });
  }
}
