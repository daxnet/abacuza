import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JobRunner } from 'app/models/job-runner';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class JobRunnersService {

  constructor(private httpClient: HttpClient) { }

  public getAllJobRunners(): Observable<HttpResponse<JobRunner[]>> {
    return this.httpClient.get<JobRunner[]>(`${environment.jobServiceBaseUrl}api/job-runners`, {
      observe: 'response',
    });
  }
}
