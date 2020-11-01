import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Project } from 'app/models/project';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProjectsService {

  constructor(private httpClient: HttpClient) { }

  public getAllProjects(): Observable<HttpResponse<Project[]>> {
    return this.httpClient.get<Project[]>(`${environment.projectServiceBaseUrl}api/projects`, {
      observe: 'response',
    });
  }

  public getProjectById(id: string): Observable<HttpResponse<Project>> {
    return this.httpClient.get<Project>(`${environment.projectServiceBaseUrl}api/projects/${id}`, {
      observe: 'response',
    });
  }

  public createProject(project: Project): Observable<string> {
    return this.httpClient.post<string>(`${environment.projectServiceBaseUrl}api/projects`, {
      name: project.name,
      description: project.description,
      inputEndpointName: project.inputEndpointName,
      jobRunnerId: project.jobRunnerId,
    });
  }
}
