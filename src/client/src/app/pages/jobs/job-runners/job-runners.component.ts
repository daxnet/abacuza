import { Component, OnInit } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { JobRunnersService } from 'app/services/job-runners.service';
import { LocalDataSource } from 'ng2-smart-table';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'ngx-job-runners',
  templateUrl: './job-runners.component.html',
  styleUrls: ['./job-runners.component.scss'],
})
export class JobRunnersComponent implements OnInit {

  settings = {
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
      saveButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      name: {
        title: 'Name',
        type: 'text',
      },
      description: {
        title: 'Description',
        type: 'text',
      },
      clusterType: {
        title: 'Cluster Type',
        type: 'text',
      },
    },
    actions: {
      edit: false,
      delete: false,
      custom: [
        {
          name: 'modify',
          title: '<i class="nb-edit" ng2-smart-actions></i>',
        },
      ],
      position: 'right',
    },
    mode: 'external',
  };

  source: LocalDataSource = new LocalDataSource();

  constructor(private jobRunnersService: JobRunnersService,
    private toastrService: NbToastrService) { }

  ngOnInit(): void {
    this.jobRunnersService.getAllJobRunners()
      .pipe(catchError(err => {
          this.toastrService.danger(`Server responded with the error message: ${err.message}`,
          'Failed to load job runners', {
          duration: 6000,
        });
        return throwError(err.message);
      }))
      .subscribe(response => this.source.load(response.body));
  }

  onCreate(): void {

  }

  onEdit(event): void {

  }

  onDelete(event): void {

  }

  onCustomAction(event): void {
    switch ( event.action) {
      case 'modify':
        break;
    }
  }

}
