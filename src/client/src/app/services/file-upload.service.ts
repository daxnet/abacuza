import { Injectable } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { S3File } from 'app/models/s3-file';
import { Observable, Subject } from 'rxjs';
import { FileUploadComponent } from './file-upload/file-upload.component';


/**
 * Represents the service for file uploading.
 *
 * @export
 * @class FileUploadService
 */
@Injectable({
  providedIn: 'root',
})
export class FileUploadService {

  constructor(private dialogService: NbDialogService) { }


  /**
   * Opens the file upload dialog and upload the selected files.
   *
   * @param {string} bucket The name of the S3 bucket to which the files should be uploaded.
   * @param {string} key The path of the file on S3.
   * @param {string} [allowedExts='.csv,.tsv,.txt'] The allowed types of the files to be uploaded.
   * @param {boolean} [allowMultiple=true] Indicates whether multiple file selection is enabled.
   * @returns {Observable<S3File[]>} A list of S3File entities containing the information of the uploaded files.
   * @memberof FileUploadService
   */
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
