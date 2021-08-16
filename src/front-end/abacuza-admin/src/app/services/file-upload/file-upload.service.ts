import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { S3File } from 'src/app/models/s3-file';
import { ComponentDialogUsage } from '../component-dialog/component-dialog-options';
import { ComponentDialogService } from '../component-dialog/component-dialog.service';
import { FileUploadComponent } from './file-upload.component';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor(private componentDialogService: ComponentDialogService) { }

  uploadFiles(bucket: string,
    key: string,
    allowedExts: string = '.csv,.tsv,.txt',
    allowMultiple: boolean = true
    ): Observable<S3File[]> {
      const subject = new Subject<S3File[]>();
      this.componentDialogService.open(FileUploadComponent, {
        bucket: bucket,
        key: key,
        allowedExts: allowedExts,
        allowMultiple: allowMultiple
      }, {
        title: 'Upload files',
        acceptButtonText: 'OK',
        usage: ComponentDialogUsage.Create
      }).subscribe(data => {
        if (data && data.s3Files) {
          subject.next(data.s3Files)
        }
      });

      return subject.asObservable();
    }
}
