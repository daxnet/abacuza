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
export class NavBarComponent extends AuthComponentBase implements OnInit {

  public user: User | null = null;
  public isAdmin: boolean = false;

  constructor(authService: AuthService) {
    super(authService);
  }

  async signOut() {
    await this.authService.logout();
  }

  async signIn() {
    await this.authService.login();
  }
}
