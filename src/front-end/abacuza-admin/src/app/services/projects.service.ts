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
}
