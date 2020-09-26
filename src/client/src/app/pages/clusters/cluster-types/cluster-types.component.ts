import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Cluster } from 'app/models/cluster';
import { ClustersService } from 'app/services/clusters.service';

@Component({
  selector: 'ngx-cluster-types',
  templateUrl: './cluster-types.component.html',
  styleUrls: ['./cluster-types.component.scss'],
})
export class ClusterTypesComponent implements OnInit {

  clusters: Cluster[] = [];

  constructor(private clustersService: ClustersService) { }

  ngOnInit(): void {
    this.clustersService.getAllClusters()
      .subscribe(response => this.clusters = response.body);
  }

}
