import { Injectable } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { S3File } from 'app/models/s3-file';
import { Observable, Subject } from 'rxjs';
import { FileUploadComponent } from './file-upload/file-upload.component';

@Injectable({
  providedIn: 'root',
})
export class FileUploadService {

  constructor(private dialogService: NbDialogService) { }

  uploadFiles(bucket: string,
    key: string,
    allowedExts: string = '.csv,.tsv,.txt',
    allowMultiple: boolean = true): Observable<S3File[]> {
    const subject = new Subject<S3File[]>();
    this.dialogService.open(FileUploadComponent, {
      context: {
        bucket,
        key,
        allowedExts,
        allowMultiple,
      },
    })
      .onClose
      .subscribe(r => subject.next(r));
    return subject.asObservable();
  }
}
