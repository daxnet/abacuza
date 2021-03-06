import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Project } from 'app/models/project';
import { ProjectRevision } from 'app/models/project-revision';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProjectsService {

  constructor(private httpClient: HttpClient) { }

  public getAllProjects(): Observable<HttpResponse<Project[]>> {
    return this.httpClient.get<Project[]>(`${environment.serviceBaseUrl}project-service/projects`, {
      observe: 'response',
    });
  }

  public getProjectById(id: string): Observable<HttpResponse<Project>> {
    return this.httpClient.get<Project>(`${environment.serviceBaseUrl}project-service/projects/${id}`, {
      observe: 'response',
    });
  }

  public createProject(project: Project): Observable<string> {
    return this.httpClient.post<string>(`${environment.serviceBaseUrl}project-service/projects`, {
      name: project.name,
      description: project.description,
      inputEndpointName: project.inputEndpointName,
      outputEndpointName: project.outputEndpointName,
      jobRunnerId: project.jobRunnerId,
    });
  }

  public deleteProject(id: string): Observable<any> {
    return this.httpClient.delete(`${environment.serviceBaseUrl}project-service/projects/${id}`);
  }

  public updateProject(id: string, entity: Project): Observable<Project> {
    return this.httpClient.patch<Project>(`${environment.serviceBaseUrl}project-service/projects/${id}`,
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
        path: '/jobRunnerId',
        value: entity.jobRunnerId,
      },
      {
        op: 'replace',
        path: '/inputEndpointName',
        value: entity.inputEndpointName,
      },
      {
        op: 'replace',
        path: '/inputEndpointSettings',
        value: entity.inputEndpointSettings,
      },
      {
        op: 'replace',
        path: '/outputEndpointName',
        value: entity.outputEndpointName,
      },
      {
        op: 'replace',
        path: '/outputEndpointSettings',
        value: entity.outputEndpointSettings,
      },
    ], {
      observe: 'body',
    });
  }

  public createRevision(projectId: string): Observable<string> {
    return this.httpClient.post<string>(`${environment.serviceBaseUrl}project-service/projects/${projectId}/revisions`, null, {
      observe: 'body',
    });
  }

  public getRevisions(projectId: string, includeJobInformation: boolean = true): Observable<ProjectRevision[]> {
    return this.httpClient.get<ProjectRevision[]>(`${environment.serviceBaseUrl}project-service/projects/${projectId}/revisions?job-info=${includeJobInformation}`, {
      observe: 'body',
    });
  }

  public getRevisionLogs(revisionId: string): Observable<string[]> {
    return this.httpClient.get<string[]>(`${environment.serviceBaseUrl}project-service/revisions/${revisionId}/logs`, {
      observe: 'body',
    });
  }

  
}
