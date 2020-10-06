import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { Cluster } from 'app/models/cluster';
import { ClustersService } from 'app/services/clusters.service';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'ngx-cluster-types',
  templateUrl: './cluster-types.component.html',
  styleUrls: ['./cluster-types.component.scss'],
})
export class ClusterTypesComponent implements OnInit {

  clusters: Cluster[] = [];

  constructor(private clustersService: ClustersService,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
    this.clustersService.getAllClusters()
      .pipe(catchError(err => {
        this.toastrService.danger(`Server responded with the error message: ${err.error}`,
          'Failed to get cluster types', {
            duration: 6000,
          });
        return throwError(err.message);
      }))
      .subscribe(response => this.clusters = response.body);
  }
}
