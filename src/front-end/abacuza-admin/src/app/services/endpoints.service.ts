import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Endpoint } from '../models/endpoint';

@Injectable({
  providedIn: 'root'
})
export class EndpointsService {

  constructor(private httpClient: HttpClient) { }

  public getEndpoints(type: string): Observable<HttpResponse<Endpoint[]>> {
    return this.httpClient.get<Endpoint[]>(`${environment.apiBaseUrl}endpoint-service/endpoints?type=${type}`, {
      observe: 'response'
    });
  }
}
