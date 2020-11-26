import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JobRunner } from 'app/models/job-runner';
import { S3File } from 'app/models/s3-file';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class JobRunnersService {

  constructor(private httpClient: HttpClient) { }

  public getAllJobRunners(): Observable<HttpResponse<JobRunner[]>> {
    return this.httpClient.get<JobRunner[]>(`${environment.jobServiceBaseUrl}api/job-runners`, {
      observe: 'response',
    }).pipe(
      map(response => {
        response.body.forEach(r => r.payloadTemplateJsonObject = JSON.parse(r.payloadTemplate));
        return response;
      })
    );
  }

  public createJobRunner(jr: JobRunner): Observable<string> {
    return this.httpClient.post<string>(`${environment.jobServiceBaseUrl}api/job-runners`, {
      name: jr.name,
      description: jr.description,
      clusterType: jr.clusterType,
    });
  }

  public getJobRunnerById(id: string): Observable<HttpResponse<JobRunner>> {
    return this.httpClient.get<JobRunner>(`${environment.jobServiceBaseUrl}api/job-runners/${id}`, {
      observe: 'response',
    }).pipe(
      map(response => {
        response.body.payloadTemplateJsonObject = JSON.parse(response.body.payloadTemplate);
        return response;
      })
    );
  }

  public deleteJobRunner(id: string): Observable<any> {
    return this.httpClient.delete(`${environment.jobServiceBaseUrl}api/job-runners/${id}`);
  }

  public addBinaryFiles(id: string, files: S3File[]): Observable<JobRunner> {
    return this.httpClient.post<JobRunner>(`${environment.jobServiceBaseUrl}api/job-runners/${id}/files`, files,
    {
      observe: 'body',
    });
  }

  public deleteBinaryFile(id: string, file: S3File): Observable<JobRunner> {
    const normalizedBucketName = encodeURIComponent(file.bucket);
    const normalizedKeyName = encodeURIComponent(file.key);
    const normalizedFileName = encodeURIComponent(file.file);
    return this.httpClient.delete<JobRunner>(`${environment.jobServiceBaseUrl}api/job-runners/${id}/files/${normalizedBucketName}/${normalizedKeyName}/${normalizedFileName}`, {
      observe: 'body',
    });
  }

  public updateJobRunner(id: string, entity: JobRunner): Observable<JobRunner> {
    entity.payloadTemplate = JSON.stringify(entity.payloadTemplateJsonObject);
    return this.httpClient.patch<JobRunner>(`${environment.jobServiceBaseUrl}api/job-runners/${id}`,
    [
      {
        op: 'replace',
        path: '/name',
        value: entity.name,
      },
      {
        op: 'replace',
        path: '/description',
        value: entity.description,
      },
      {
        op: 'replace',
        path: '/clusterType',
        value: entity.clusterType,
      },
      {
        op: 'replace',
        path: '/payloadTemplate',
        value: entity.payloadTemplate,
      },
    ], {
      observe: 'body',
    });
  }
}
