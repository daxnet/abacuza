import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { JobRunner } from '../models/job-runner';
import { S3File } from '../models/s3-file';

@Injectable({
  providedIn: 'root'
})
export class JobRunnersService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Gets a list of all job runners.
   *
   * @returns {Observable<HttpResponse<JobRunner[]>>}
   * @memberof JobRunnersService
   */
  public getJobRunners(): Observable<HttpResponse<JobRunner[]>> {
    return this.httpClient.get<JobRunner[]>(`${environment.apiBaseUrl}job-service/job-runners`, {
      observe: 'response'
    }).pipe(map(response => {
      response.body?.forEach(r => r.payloadTemplateJsonObject = JSON.parse(r.payloadTemplate));
      return response;
    }))
  }


  /**
   *
   * Retrieves a job runner by its ID.
   *
   * @param {string} id
   * @returns {Observable<HttpResponse<JobRunner>>}
   * @memberof JobRunnersService
   */
  public getJobRunnerById(id: string): Observable<HttpResponse<JobRunner>> {
    return this.httpClient.get<JobRunner>(`${environment.apiBaseUrl}job-service/job-runners/${id}`, {
      observe: 'response',
    }).pipe(
      map(response => {
        if (response && response.body) {
          response.body.payloadTemplateJsonObject = JSON.parse(response.body.payloadTemplate);
        }
        return response;
      })
    );
  }

  public createJobRunner(jr: JobRunner): Observable<string> {
    return this.httpClient.post<string>(`${environment.apiBaseUrl}job-service/job-runners`, {
      name: jr.name,
      description: jr.description,
      clusterType: jr.clusterType,
    });
  }

  public deleteJobRunner(id: string): Observable<any> {
    return this.httpClient.delete(`${environment.apiBaseUrl}job-service/job-runners/${id}`);
  }

  public addBinaryFiles(id: string, files: S3File[]): Observable<JobRunner> {
    return this.httpClient.post<JobRunner>(`${environment.apiBaseUrl}job-service/job-runners/${id}/files`, files,
    {
      observe: 'body',
    });
  }

  public deleteBinaryFile(id: string, file: S3File): Observable<JobRunner> {
    const normalizedBucketName = encodeURIComponent(file.bucket);
    const normalizedKeyName = encodeURIComponent(file.key);
    const normalizedFileName = encodeURIComponent(file.file);
    return this.httpClient.delete<JobRunner>(`${environment.apiBaseUrl}job-service/job-runners/${id}/files/${normalizedBucketName}/${normalizedKeyName}/${normalizedFileName}`, {
      observe: 'body',
    });
  }

  public updateJobRunner(id: string, entity: JobRunner): Observable<JobRunner> {
    entity.payloadTemplate = JSON.stringify(entity.payloadTemplateJsonObject);
    return this.httpClient.patch<JobRunner>(`${environment.apiBaseUrl}job-service/job-runners/${id}`,
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
