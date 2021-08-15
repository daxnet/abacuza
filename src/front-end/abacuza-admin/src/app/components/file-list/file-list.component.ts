import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { S3File } from 'src/app/models/s3-file';
import { CommonDialogResult, CommonDialogType } from 'src/app/services/common-dialog/common-dialog-data-types';
import { CommonDialogService } from 'src/app/services/common-dialog/common-dialog.service';
import { FileUploadService } from 'src/app/services/file-upload/file-upload.service';
import { ToastService } from 'src/app/services/toast/toast.service';

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss']
})
export class FileListComponent implements OnInit, OnDestroy, AfterViewInit {

  @Input() id: string = 'fileListComponent';
  @Input() title?: string;
  @Input() initFiles: S3File[] = [];
  @Input() bucket: string = 'data';
  @Input() key: string = '';
  @Input() allowedExts?: string;
  @Input() allowMultipleSelection?: boolean;

  @Output() onFileAdded: EventEmitter<S3File[]> = new EventEmitter<S3File[]>();
  @Output() onFileDeleted: EventEmitter<S3File> = new EventEmitter<S3File>();
  @Output() onFilesChangedCompleted: EventEmitter<S3File[]> = new EventEmitter<S3File[]>();

  dtTableOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();

  @ViewChild(DataTableDirective, { static: false })
  dtElement!: DataTableDirective;

  constructor(private fileUploadService: FileUploadService,
    private toast: ToastService,
    private commonDialogService: CommonDialogService) { }

  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  ngOnInit(): void {
    this.dtTableOptions = {
      pagingType: 'full_numbers',
      columnDefs: [{
        targets: 1,
        orderable: false
      }]
    };

    this.dtTrigger.next();
  }

  onAddFile(): void {
    this.fileUploadService.uploadFiles(this.bucket, this.key, this.allowedExts, this.allowMultipleSelection)
      .pipe(catchError(err => {
        this.toast.error('Failed to upload files.');
        return throwError(err.message);
      }))
      .subscribe(files => {
        if (files) {
          this.onFileAdded.emit(files);
          if (!this.initFiles) {
            this.initFiles = [];
          }

          files.forEach(file => {
            if (!this.initFiles.find(f => f.bucket === file.bucket && f.file === file.file && f.key === file.key)) {
              this.initFiles.push(file);
            }
          });
          this.rerender();
          this.onFilesChangedCompleted.emit(files);
        }
      });
  }

  onDeleteFile(event: any): void {
    this.commonDialogService.open('Confirm', 'Are you sure you want to delete the selected file?', CommonDialogType.Confirm)
      .subscribe(dr => {
        if (dr) {
          switch (dr) {
            case CommonDialogResult.Yes:
              const s3File: S3File = event;
              this.onFileDeleted.emit(event);
              if (!this.initFiles) {
                this.initFiles = [];
              }

              const idx = this.initFiles.findIndex(f => f.bucket === s3File.bucket && f.file === s3File.file && f.key === s3File.key);
              if (idx > -1) {
                this.initFiles.splice(idx, 1);
              }
              this.rerender();
              this.onFilesChangedCompleted.emit(this.initFiles);
              break;
          }
        }
      })
  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }
}
