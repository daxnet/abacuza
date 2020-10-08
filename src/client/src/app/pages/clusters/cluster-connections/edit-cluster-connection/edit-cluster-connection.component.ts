import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { ClusterConnection } from 'app/models/cluster-connection';

@Component({
  selector: 'ngx-edit-cluster-connection',
  templateUrl: './edit-cluster-connection.component.html',
  styleUrls: ['./edit-cluster-connection.component.scss'],
})
export class EditClusterConnectionComponent implements OnInit {

  @Input() title: string;
  @Input() clusterTypes: string[];
  @Input() clusterConnectionEntity: ClusterConnection;
  @Input() mode: string;

  selectedClusterType: string;

  constructor(protected ref: NbDialogRef<EditClusterConnectionComponent>) { }

  ngOnInit(): void {

  }

  close() {
    this.ref.close();
  }

  submit() {
    this.ref.close(this.clusterConnectionEntity);
  }
}
