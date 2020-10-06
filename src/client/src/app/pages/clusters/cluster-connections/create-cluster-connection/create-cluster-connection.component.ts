import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'ngx-create-cluster-connection',
  templateUrl: './create-cluster-connection.component.html',
  styleUrls: ['./create-cluster-connection.component.scss']
})
export class CreateClusterConnectionComponent implements OnInit {

  @Input() title: string;
  @Input() clusterTypes: string[];

  selectedClusterType: string;
  
  constructor(protected ref: NbDialogRef<CreateClusterConnectionComponent>) { }

  ngOnInit(): void {
    // this.selectedClusterType = this.clusterTypes[0];
  }

  dismiss() {
    this.ref.close();
  }
}
