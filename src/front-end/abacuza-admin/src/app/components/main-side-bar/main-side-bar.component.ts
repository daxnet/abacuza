import { Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'oidc-client';
import { AuthComponentBase } from 'src/app/classes/auth-component-base';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-main-side-bar',
  templateUrl: './main-side-bar.component.html',
  styleUrls: ['./main-side-bar.component.scss']
})
export class MainSideBarComponent implements OnInit {

  public user: User | null = null;
  public isAdmin: boolean = false;

  constructor (private authService: AuthService) {
    this.authService.loginChanged
      .subscribe(authenticated => {
        if (authenticated) {
          this.user = this.authService.currentUser;
          this.isAdmin = this.authService.isAdmin;
        } else {
          this.user = null;
        }
      });
  }

  ngOnInit(): void {
    this.authService.isAuthenticated()
      .then(_ => {
        this.user = this.authService.currentUser;
        this.isAdmin = this.authService.isAdmin;
      });
  }
}
