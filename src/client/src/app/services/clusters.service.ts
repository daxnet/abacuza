import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cluster } from '../models/cluster';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ClustersService {

  constructor(private http: HttpClient) { }

  /**
   * getAllClusters
   */
   public getAllClusters(): Observable<HttpResponse<Cluster[]>> {
    return this.http.get<Cluster[]>(`${environment.serviceBaseUrl}cluster-service/clusters`, {
      observe: 'response',
    });
  }

  public getAllClusterTypes(): Observable<HttpResponse<string[]>> {
    return this.http.get<string[]>(`${environment.serviceBaseUrl}cluster-service/clusters/types`, {
      observe: 'response',
    });
  }
}
