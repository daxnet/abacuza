import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Project } from '../models/project';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(private httpClient: HttpClient) { }

  public getProjects(): Observable<HttpResponse<Project[]>> {
    return this.httpClient.get<Project[]>(`${environment.apiBaseUrl}project-service/projects`, {
      observe: 'response',
    });
  }

  public getProjectById(id: string): Observable<HttpResponse<Project>> {
    return this.httpClient.get<Project>(`${environment.apiBaseUrl}project-service/projects/${id}`, {
      observe: 'response',
    });
  }

  public createProject(project: Project): Observable<string> {
    return this.httpClient.post<string>(`${environment.apiBaseUrl}project-service/projects`, {
      name: project.name,
      description: project.description,
      jobRunnerId: project.jobRunnerId,
      inputEndpoints: project.inputEndpoints,
      selectedOutputEndpointId: project.selectedOutputEndpointId,
      outputEndpoints: project.outputEndpoints
    });
  }

  public createRevision(projectId: string): Observable<string> {
    return this.httpClient.post<string>(`${environment.apiBaseUrl}project-service/projects/${projectId}/revisions`, null, {
      observe: 'body'
    });
  }

  public updateProject(id: string, entity: Project): Observable<Project> {
    return this.httpClient.patch<Project>(`${environment.apiBaseUrl}project-service/projects/${id}`,
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
        path: '/inputEndpoints',
        value: entity.inputEndpoints,
      },
      {
        op: 'replace',
        path: '/selectedOutputEndpointId',
        value: entity.selectedOutputEndpointId
      },
      {
        op: 'replace',
        path: '/outputEndpoints',
        value: entity.outputEndpoints,
      }
    ], {
      observe: 'body',
    });
  }

  public deleteProject(id: string): Observable<any> {
    return this.httpClient.delete(`${environment.apiBaseUrl}project-service/projects/${id}`);
  }
}
