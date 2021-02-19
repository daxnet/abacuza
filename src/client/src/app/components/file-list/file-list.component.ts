import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { CommonDialogResult } from 'app/models/common-dialog-result';
import { S3File } from 'app/models/s3-file';
import { CommonDialogService } from 'app/services/common-dialog.service';
import { FileUploadService } from 'app/services/file-upload.service';
import { LocalDataSource } from 'ng2-smart-table';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'ngx-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss'],
})
export class FileListComponent implements OnInit {

  @Input() id: string;
  @Input() title?: string;
  @Input() initFiles: S3File[] = [];
  @Input() bucket: string;
  @Input() key: string;
  @Input() allowedExts?: string;
  @Input() allowMultipleSelection?: boolean;

  @Output() onFileAdded: EventEmitter<S3File[]> = new EventEmitter<S3File[]>();
  @Output() onFileDeleted: EventEmitter<S3File> = new EventEmitter<S3File>();
  @Output() onFilesChangedCompleted: EventEmitter<S3File[]> = new EventEmitter<S3File[]>();

  filesTableSettings = {
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      bucket: {
        title: 'Bucket',
        type: 'text',
      },
      key: {
        title: 'Key',
        type: 'text',
      },
      file: {
        title: 'File name',
        type: 'text',
      },
    },
    actions: {
      edit: false,
      position: 'right',
    },
    mode: 'external',
  };

  source: LocalDataSource = new LocalDataSource();

  constructor(private fileUploadService: FileUploadService,
    private commonDialogService: CommonDialogService,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
    if (!this.initFiles) {
      this.initFiles = [];
    }

    this.source.load(this.initFiles);
  }

  onAddFile(): void {
    this.fileUploadService.uploadFiles(this.bucket, this.key, this.allowedExts, this.allowMultipleSelection)
      .pipe(catchError(err => {
        this.toastrService.danger(err.message, 'Failed to upload files.');
        return throwError(err.message);
      }))
      .subscribe(files => {
        if (files) {
          this.onFileAdded.emit(files);
          // Merge the newly selected file into the initFiles array
          files.forEach(file => {
            if (!this.initFiles.find(f => f.bucket === file.bucket && f.file === file.file && f.key === file.key)) {
              this.initFiles.push(file);
            }
          });
          this.source.load(this.initFiles);
          this.source.refresh();
          this.onFilesChangedCompleted.emit(this.initFiles);
        }
      });
  }

  onDeleteFile(event): void {
    this.commonDialogService.confirm('Delete file(s)', `Are you sure you want to delete ${event.data.file}?`)
      .subscribe(dr => {
        if (dr && dr === CommonDialogResult.Yes) {
          const s3File: S3File = event.data;
          this.onFileDeleted.emit(event.data);
          const idx = this.initFiles.findIndex(f =>
            f.bucket === s3File.bucket && f.file === s3File.file && f.key === s3File.key);
          if (idx > -1) {
            this.initFiles.splice(idx, 1);
          }

          this.source.load(this.initFiles);
          this.source.refresh();

          this.onFilesChangedCompleted.emit(this.initFiles);
        }
      });
  }
}
