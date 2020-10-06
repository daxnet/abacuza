import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ClusterConnection } from 'app/models/cluster-connection';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClusterConnectionsService {

  constructor(private httpClient: HttpClient) { }

  public getAllClusterConnections(): Observable<HttpResponse<ClusterConnection[]>> {
    return this.httpClient.get<ClusterConnection[]>(`${environment.clusterServiceBaseUrl}api/cluster-connections`, {
      observe: 'response',
    });
  }
}
