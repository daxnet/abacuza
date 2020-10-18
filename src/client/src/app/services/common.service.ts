import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommonService {

  constructor(private http: HttpClient) { }

  public uploadFileToS3(file: File, bucket: string, key: string): Observable<HttpEvent<any>> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    formData.append('bucket', bucket);
    formData.append('key', key);
    return this.http.post(`${environment.commonServiceBaseUrl}api/files/s3`, formData, {
      reportProgress: true, observe: 'events',
    });
  }
}
