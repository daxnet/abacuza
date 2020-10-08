import { Component, OnInit } from '@angular/core';
import { NbDialogService, NbToastrService } from '@nebular/theme';
import { ClusterConnection } from 'app/models/cluster-connection';
import { CommonDialogResult } from 'app/models/common-dialog-result';
import { ClusterConnectionsService } from 'app/services/cluster-connections.service';
import { ClustersService } from 'app/services/clusters.service';
import { CommonDialogService } from 'app/services/common-dialog.service';
import { LocalDataSource } from 'ng2-smart-table';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EditClusterConnectionComponent } from './edit-cluster-connection/edit-cluster-connection.component';

@Component({
  selector: 'ngx-cluster-connections',
  templateUrl: './cluster-connections.component.html',
  styleUrls: ['./cluster-connections.component.scss'],
})
export class ClusterConnectionsComponent implements OnInit {


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
      position: 'right',
    },
    mode: 'external',
  };

  source: LocalDataSource = new LocalDataSource();
  clusterTypes: string[] = [];

  constructor(private clusterConnectionsService: ClusterConnectionsService,
    private clustersService: ClustersService,
    private dialogService: NbDialogService,
    private toastrService: NbToastrService,
    private commonDialogService: CommonDialogService) {
    this.clusterConnectionsService.getAllClusterConnections()
      .pipe(catchError(err => {
        this.toastrService.danger(`Server responded with the error message: ${err.message}`,
          'Failed to load cluster connections', {
          duration: 6000,
        });
        return throwError(err.message);
      }))
      .subscribe(response => {
        this.source.load(response.body);
      });
    this.clustersService.getAllClusterTypes()
      .subscribe(response => this.clusterTypes = response.body);
  }

  ngOnInit(): void {
  }

  onEdit(event): void {
    this.dialogService.open(EditClusterConnectionComponent, {
      context: {
        title: 'Edit Cluster Connection',
        clusterTypes: this.clusterTypes,
        clusterConnectionEntity: event.data,
        mode: 'edit',
      },
      closeOnBackdropClick: false,
    })
    .onClose
    .subscribe(res => {
      if (res) {
        this.clusterConnectionsService.updateClusterConnection(event.data.id,
            event.data.description,
            event.data.settings)
          .pipe(catchError(err => {
            this.toastrService.danger(`Error message: ${err.message}`, 'Failed to update cluster connection', {
              duration: 6000,
            });
            return throwError(err.message);
          }))
          .subscribe(response => {
            this.toastrService.success('Update cluster connection successfully.', 'Success');
            this.source.update(event.data, response);
            this.source.refresh();
          });
      }
    });
  }

  onDelete(event): void {
    this.commonDialogService.confirm('Delete Cluster Connection', 'Are you sure to delete the current cluster connection?')
      .subscribe(dr => {
        if (dr === CommonDialogResult.Yes) {
          this.clusterConnectionsService.deleteClusterConnection(event.data.id)
            .pipe(catchError(err => {
              this.toastrService.danger(`Error message: ${err.message}`, 'Failed to delete cluster connection', {
                duration: 6000,
              });
              return throwError(err.message);
            }))
            .subscribe(_ => {
              this.toastrService.success('Cluster connection deleted successfully.', 'Success');
              this.source.remove(event.data);
              this.source.refresh();
            });
        }
      });
  }

  onCreate(): void {
    this.dialogService.open(EditClusterConnectionComponent, {
      context: {
        title: 'Create New Cluster Connection',
        clusterTypes: this.clusterTypes,
        clusterConnectionEntity: new ClusterConnection(),
      },
      closeOnBackdropClick: false,
    })
      .onClose.subscribe(res => {
        if (res) {
          this.clusterConnectionsService.createClusterConnection(res)
            .pipe(catchError(err => {
              this.toastrService.danger(`Server responded with the error message: ${err.message}`,
                'Failed to create cluster connection', {
                duration: 6000,
              });
              return throwError(err.message);
            }))
            .subscribe(responseId => {
              res.id = responseId;
              this.toastrService.success('Cluster connection created successfully.', 'Success');
              this.source.add(res).then(_ => this.source.refresh());
            });
        }
      });
  }
}
