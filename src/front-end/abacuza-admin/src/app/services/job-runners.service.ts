import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { JobRunner } from '../models/job-runner';

@Injectable({
  providedIn: 'root'
})
export class JobRunnersService {

  constructor(private httpClient: HttpClient) { }

  public getJobRunners(): Observable<HttpResponse<JobRunner[]>> {
    return this.httpClient.get<JobRunner[]>(`${environment.apiBaseUrl}job-service/job-runners`, {
      observe: 'response'
    }).pipe(map(response => {
      response.body?.forEach(r => r.payloadTemplateJsonObject = JSON.parse(r.payloadTemplate));
      return response;
    }))
  }
}
