import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Endpoint } from 'app/models/endpoint';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EndpointsService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Retrieves a list of available endpoints by using the specified type.
   *
   * @param {string} [type='input'] The type of the endpoint to be retrieved.
   * @returns {Observable<HttpResponse<Endpoint[]>>}
   * @memberof EndpointsService
   */
  public getAvailableEndpoints(type: string = 'input'): Observable<HttpResponse<Endpoint[]>> {
    return this.httpClient.get<Endpoint[]>(`${environment.endpointServiceBaseUrl}api/endpoints?type=${type}`, {
      observe: 'response',
    });
  }

  public getEndpointByName(name: string): Observable<HttpResponse<Endpoint>> {
    return this.httpClient.get<Endpoint>(`${environment.endpointServiceBaseUrl}api/endpoints/${name}`, {
      observe: 'response',
    });
  }
}
