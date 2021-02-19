import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ClusterConnection } from 'app/models/cluster-connection';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ClusterConnectionsService {

  constructor(private httpClient: HttpClient) { }

  public getAllClusterConnections(): Observable<HttpResponse<ClusterConnection[]>> {
    return this.httpClient.get<ClusterConnection[]>(`${environment.clusterServiceBaseUrl}api/cluster-connections`, {
      observe: 'response',
    }).pipe(
      map(response => {
        response.body.forEach(r => r.settingsJsonObject = JSON.parse(r.settings));
        return response;
      },)
    );
  }

  public createClusterConnection(conn: ClusterConnection): Observable<string> {
    return this.httpClient.post<string>(`${environment.clusterServiceBaseUrl}api/cluster-connections`, {
      clusterType: conn.clusterType,
      description: conn.description,
      name: conn.name,
      settings: conn.settings,
    });
  }

  public deleteClusterConnection(id: string): Observable<any> {
    return this.httpClient.delete(`${environment.clusterServiceBaseUrl}api/cluster-connections/${id}`);
  }

  public updateClusterConnection(id: string, description: string, settingsJsonObject: any): Observable<ClusterConnection> {

    return this.httpClient.patch<ClusterConnection>(
      `${environment.clusterServiceBaseUrl}api/cluster-connections/${id}`,
      [
        {
          op: 'replace',
          path: '/description',
          value: description,
        },
        {
          op: 'replace',
          path: '/settings',
          value: JSON.stringify(settingsJsonObject),
        },
      ], {
      observe: 'body',
    });
  }
}
