import { ChangeDetectorRef } from '@angular/core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'oidc-client';
import { Subscription } from 'rxjs';
import { AuthComponentBase } from 'src/app/classes/auth-component-base';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  public user: User | null = null;
  public isAdmin: boolean = false;
  public isAuthenticated: boolean = false;

  constructor(private authService: AuthService) {
    this.authService.loginChanged
      .subscribe(authenticated => {
        this.isAuthenticated = authenticated;
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
      .then(authenticated => {
        this.isAuthenticated = authenticated;
        if (authenticated) {
          this.user = this.authService.currentUser;
          this.isAdmin = this.authService.isAdmin;
        } else {
          this.isAdmin = false;
          this.user = null;
        }
      });
  }

  async signOut() {
    await this.authService.logout();
  }

  async signIn() {
    await this.authService.login();
  }
}
