import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { S3File } from 'app/models/s3-file';
import { environment } from 'environments/environment';
import { CommonService } from '../common.service';

@Component({
  selector: 'ngx-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss'],
})
export class FileUploadComponent implements OnInit {

  @Input() bucket: string;
  @Input() key: string;
  @Input() allowedExts: string = '.csv,.tsv,.txt';
  @Input() allowMultiple: boolean = true;

  s3Files: S3File[] = [];

  selectedFiles: FileList;
  progressInfos = [];

  constructor(protected ref: NbDialogRef<FileUploadComponent>,
    private commonService: CommonService) { }

  ngOnInit(): void {
  }

  selectFiles(event): void {
    this.progressInfos = [];
    this.selectedFiles = event.target.files;
  }

  upload(idx: number, file: File): void {
    this.progressInfos[idx] = { value: 0, fileName: file.name, mstyle: undefined, message: undefined };
    this.commonService.uploadFileToS3(file, this.bucket, this.key).subscribe(
      event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progressInfos[idx].value = Math.round(100 * event.loaded / event.total);
        } else if (event instanceof HttpResponse) {
          this.s3Files.push(event.body[0]);
          this.progressInfos[idx].mstyle = 'text-success';
          this.progressInfos[idx].message = 'Success';
        }
      },
      err => {
        this.progressInfos[idx].mstyle = 'text-danger';
        this.progressInfos[idx].message = `Failed: ${err.statusText}`;
      });
  }

  uploadFiles(): void {
    for (let i = 0; i < this.selectedFiles.length; i++) {
      this.upload(i, this.selectedFiles[i]);
    }
  }

  ok(): void {
    this.ref.close(this.s3Files);
  }

  cancel(): void {
    this.ref.close();
  }
}
