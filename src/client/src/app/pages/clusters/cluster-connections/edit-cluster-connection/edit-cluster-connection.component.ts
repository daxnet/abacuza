import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { JsonEditorOptions } from 'ang-jsoneditor';
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

  editorOptions: JsonEditorOptions;

  constructor(protected ref: NbDialogRef<EditClusterConnectionComponent>) {
    this.editorOptions = new JsonEditorOptions();
    this.editorOptions.mode = 'code';
    this.editorOptions.mainMenuBar = false;
  }

  ngOnInit(): void {
  }

  close() {
    this.ref.close();
  }

  submit() {
    this.ref.close(this.clusterConnectionEntity);
  }
}
