import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject, Subscription } from 'rxjs';
import { AbacuzaUser } from 'src/app/models/abacuza-user';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, OnDestroy {

  dtTableOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  subscriptions: Subscription[] = [];
  users: AbacuzaUser[] | null = [];

  @ViewChild(DataTableDirective, { static: false })
  dtElement!: DataTableDirective;
  
  constructor(private authService: AuthService) { }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  ngOnInit(): void {
    this.dtTableOptions = {
      pagingType: 'full_numbers'
    };

    this.subscriptions.push(this.authService.getUsers()
      .subscribe(response => {
        this.users = response.body;
        this.dtTrigger.next();
      }))
  }

  private rerender(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.destroy();
      this.dtTrigger.next();
    });
  }

}
