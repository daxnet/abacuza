import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ClustersService {

  constructor(private httpClient: HttpClient, private authService: AuthService) { }

  public getAllClusterTypes(): Observable<HttpResponse<string[]>> {
    return this.httpClient.get<string[]>(`${environment.apiBaseUrl}cluster-service/clusters/types`, {
      observe: 'response'
    });
  }
}
