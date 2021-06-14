import { Component, OnInit } from '@angular/core';
import { User } from 'oidc-client';
import { AuthService } from 'src/app/services/auth.service';
import { ClustersService } from 'src/app/services/clusters.service';

@Component({
  selector: 'app-installed-plugins',
  templateUrl: './installed-plugins.component.html',
  styleUrls: ['./installed-plugins.component.scss']
})
export class InstalledPluginsComponent implements OnInit {

  clusterTypes: string[] | null = null;

  constructor(private clustersService: ClustersService) {

  }

  ngOnInit(): void {
    this.clustersService.getAllClusterTypes()
      .subscribe(response => {
        this.clusterTypes = response.body;
        console.log('Retrieving cluster information successfully.');
      });
  }

}
