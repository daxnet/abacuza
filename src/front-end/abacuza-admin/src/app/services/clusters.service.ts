import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Cluster } from '../models/cluster';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ClusterConnection } from '../models/cluster-connection';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ClustersService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Retrieves all registered clusters.
   *
   * @returns {Observable<HttpResponse<Cluster>>}
   * @memberof ClustersService
   */
  public getClusters(): Observable<HttpResponse<Cluster[]>> {
    return this.httpClient.get<Cluster[]>(`${environment.apiBaseUrl}cluster-service/clusters`, {
      observe: 'response'
    });
  }

  public getClusterTypes(): Observable<HttpResponse<string[]>> {
    return this.httpClient.get<string[]>(`${environment.apiBaseUrl}cluster-service/clusters/types`, {
      observe: 'response'
    });
  }

  public getClusterConnections(): Observable<HttpResponse<ClusterConnection[]>> {
    return this.httpClient.get<ClusterConnection[]>(`${environment.apiBaseUrl}cluster-service/cluster-connections`, {
      observe: 'response'
    }).pipe(
      map(response => {
        response.body?.forEach(r => r.settingsJsonObject = r.settings ? JSON.parse(r.settings!) : {});
        return response;
      })
    );
  }

  public createClusterConnection(conn: ClusterConnection): Observable<string> {
    return this.httpClient.post<string>(`${environment.apiBaseUrl}cluster-service/cluster-connections`, {
      clusterType: conn.clusterType,
      description: conn.description,
      name: conn.name,
      settings: conn.settings
    });
  }

  public deleteClusterConnection(id: string): Observable<any> {
    return this.httpClient.delete(`${environment.apiBaseUrl}cluster-service/cluster-connections/${id}`);
  }


  /**
   *
   * Updates the cluster connection.
   * 
   * @param {string} id The ID of the cluster connection.
   * @param {string} description The description of the cluster connection.
   * @param {*} settingsJsonObject The JSON object that represents the settings of the cluster connection.
   * @returns {Observable<ClusterConnection>} The updated cluster connection.
   * @memberof ClustersService
   */
  public updateClusterConnection(id: string, description: string, settingsJsonObject: any): Observable<ClusterConnection> {
    return this.httpClient.patch<ClusterConnection>(
      `${environment.apiBaseUrl}cluster-service/cluster-connections/${id}`,
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
