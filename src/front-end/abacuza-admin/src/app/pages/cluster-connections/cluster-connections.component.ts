import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject, Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ClusterConnection } from 'src/app/models/cluster-connection';
import { ClustersService } from 'src/app/services/clusters.service';
import { CommonDialogResult, CommonDialogType } from 'src/app/services/common-dialog/common-dialog-data-types';
import { CommonDialogService } from 'src/app/services/common-dialog/common-dialog.service';
import { ComponentDialogUsage } from 'src/app/services/component-dialog/component-dialog-options';
import { ComponentDialogService } from 'src/app/services/component-dialog/component-dialog.service';
import { ToastService } from 'src/app/services/toast/toast.service';
import { EditClusterConnectionComponent } from '../edit-cluster-connection/edit-cluster-connection.component';

@Component({
  selector: 'app-cluster-connections',
  templateUrl: './cluster-connections.component.html',
  styleUrls: ['./cluster-connections.component.scss']
})
export class ClusterConnectionsComponent implements OnInit, OnDestroy {

  dtTableOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  clusterConnections: ClusterConnection[] | null = null;
  clusterTypes: string[] | null = null;
  
  private subscriptions: Subscription[] = [];

  @ViewChild(DataTableDirective, { static: false })
  dtElement!: DataTableDirective;

  constructor(private clustersService: ClustersService,
    private commonDialogService: CommonDialogService,
    private toastService: ToastService,
    private componentDialogService: ComponentDialogService) { }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.subscriptions?.forEach(s => s.unsubscribe());
  }

  ngOnInit(): void {
    this.dtTableOptions = {
      pagingType: 'full_numbers'
    };

    this.subscriptions.push(this.clustersService.getClusterConnections()
      .subscribe(response => {
        this.clusterConnections = response.body;
        this.dtTrigger.next();
      }));

    this.subscriptions.push(this.clustersService.getClusterTypes()
      .subscribe(response => {
        this.clusterTypes = response.body;
      }));
  }

  onDeleteClicked(event: any) {
    this.subscriptions.push(this.commonDialogService.open('Confirm', 'Are you sure you want to delete the selected connection?', CommonDialogType.Confirm)
      .subscribe(f => {
        switch (f) {
          case CommonDialogResult.Yes:
            this.clustersService.deleteClusterConnection(event.id)
              .pipe(catchError(err => {
                this.toastService.error(err.message);
                return throwError(err.message);
              }))
              .subscribe(() => {
                this.toastService.success('Cluster connection was deleted successfully.');
                this.clusterConnections = this.clusterConnections?.filter(x => x.id !== event.id) ?? null;
                this.rerender();
              })
            break;
        }
      }));
  }

  onAddClicked() {
    this.subscriptions.push(this.componentDialogService.open(EditClusterConnectionComponent, {
      clusterTypes: this.clusterTypes,
      clusterConnection: {
        clusterType: this.clusterTypes![0]
      }
    }, {
      title: 'Add Connection',
      usage: ComponentDialogUsage.Create
    }).subscribe(result => {
      if (result) {
        const clusterConnection = result.clusterConnection as ClusterConnection;
        this.subscriptions.push(this.clustersService.createClusterConnection(clusterConnection)
          .pipe(catchError(err => {
            this.toastService.error(`Failed to create cluster connection. ${err.message}`);
            return throwError(err.message);
          }))
          .subscribe(id => {
            clusterConnection.id = id;
            this.toastService.success('Cluster connection created successfully.');
            this.clusterConnections?.push(clusterConnection);
            this.rerender();
          }));
      }
    }));
  }

  onEditClicked(event: any) {
    this.subscriptions.push(this.componentDialogService.open(EditClusterConnectionComponent, {
      clusterTypes: this.clusterTypes,
      clusterConnection: event
    }, {
      title: 'Edit Connection',
      usage: ComponentDialogUsage.Edit
    }).subscribe(result => {
        if (result) {
          const clusterConnection = result.clusterConnection as ClusterConnection;
          this.subscriptions.push(this.clustersService.updateClusterConnection(
            clusterConnection.id, 
            clusterConnection.description!, 
            clusterConnection.settingsJsonObject)
          .pipe(catchError(err => {
            this.toastService.error('Failed to update cluster connection.');
            return throwError(err.message);
          }))
          .subscribe(_ => {
            this.toastService.success('Cluster connection updated successfully.');
            const idx = this.clusterConnections!.findIndex(x => x.id === clusterConnection.id);
            this.clusterConnections![idx] = clusterConnection;
            this.rerender();
          }));
        }
    }));
  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }
}
