import { Component, OnInit } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { ClusterConnectionsService } from 'app/services/cluster-connections.service';
import { LocalDataSource } from 'ng2-smart-table';
import { CreateClusterConnectionComponent } from './create-cluster-connection/create-cluster-connection.component';

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

  constructor(private clusterConnectionsService: ClusterConnectionsService,
    private dialogService: NbDialogService) {
    this.clusterConnectionsService.getAllClusterConnections()
      .subscribe(response => this.source.load(response.body));
  }

  ngOnInit(): void {
  }

  open() {
    this.dialogService.open(CreateClusterConnectionComponent, {
      context: {
        title: 'This is a title passed to the dialog component',
      },
    });
  }

  onDeleteConfirm(event): void {
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();
    } else {
      event.confirm.reject();
    }
  }

  onCreate(event): void {
    this.dialogService.open(CreateClusterConnectionComponent, {
      context: {
        title: 'Create New Cluster Connection',
        clusterTypes: ['spark', '.net'],
      },
    });
  }
}
