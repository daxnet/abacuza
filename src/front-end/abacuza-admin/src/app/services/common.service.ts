import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private http: HttpClient) { }

  public uploadFileToS3(file: File, bucket: string, key: string): Observable<HttpEvent<any>> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    formData.append('bucket', bucket);
    formData.append('key', key);
    return this.http.post(`${environment.apiBaseUrl}common-service/files/s3`, formData, {
      reportProgress: true, observe: 'events',
    });
  }
}
