import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { S3File } from 'src/app/models/s3-file';
import { CommonService } from '../common.service';
import { ComponentDialogBase } from '../component-dialog/component-dialog-base';
import { ComponentDialogUsage } from '../component-dialog/component-dialog-options';
import { FileUploadProgressInfo } from './file-upload-progress-info';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit, ComponentDialogBase {

  @Input() usage?: ComponentDialogUsage | undefined;
  @Input() data: any;

  s3Files: S3File[] = [];
  selectedFiles?: FileList;
  progressInfos: FileUploadProgressInfo[] = [];

  constructor(private commonService: CommonService) { }
  

  ngOnInit(): void {
  }

  selectFiles(event: any): void {
    this.progressInfos = [];
    this.selectedFiles = event.target.files;
  }

  uploadFiles(): void {
    if (this.selectedFiles) {
      for (let i = 0; i < this.selectedFiles.length; i++) {
        this.upload(i, this.selectedFiles[i]);
      }

      this.data.s3Files = this.s3Files;
    }
  }

  private upload(idx: number, file: File): void {
    this.progressInfos[idx] = { value: 0, fileName: file.name, mstyle: undefined, message: undefined };
    this.commonService.uploadFileToS3(file, this.data.bucket, this.data.key).subscribe(
      event => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
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
}
