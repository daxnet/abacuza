import { Injectable } from '@angular/core';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginChangedSubject$ = new Subject<boolean>();
  private userManager : UserManager;
  private user: User | null = null;

  public loginChanged = this.loginChangedSubject$.asObservable();

  constructor() {
    this.userManager  = new UserManager(this.getUserManagerSettings());
  }

  public async login() {
    await this.userManager.signinRedirect();
  }

  public async logout() {
    await this.userManager.signoutRedirect();
  }

  public get currentUser(): User | null {
    return this.user;
  }

  public async completeAuthentication(): Promise<void> {
    const user = await this.userManager.signinRedirectCallback();
    if (this.user !== user) {
      this.user = user;
      this.loginChangedSubject$.next(this.checkUser(user));
    }
  }

  public get authorizationHeaderValue(): string {
    return `${this.user?.token_type} ${this.user?.access_token}`;
  }

  public isAuthenticated = async (): Promise<boolean> => {
    const user = await this.userManager.getUser();
    if (this.user !== user) {
      this.loginChangedSubject$.next(this.checkUser(user));
      this.user = user;
    }
    return this.checkUser(user);
  }

  public userHasRole(role: string): boolean {
    const roles: string[] | null = this.user == null ? [] : this.user.profile.role;
    if (roles) {
      return roles.findIndex(item => item === role) >= 0;
    }
    return false;
  }

  public get isAdmin(): boolean {
    return this.userHasRole('admin');
  }

  private checkUser = (user: User | null): boolean => !!user && !user.expired;

  private getUserManagerSettings(): UserManagerSettings {
    return {
      authority: environment.idpAuthority,
      client_id: environment.idpClientId,
      redirect_uri: environment.idpRedirectUrl,
      post_logout_redirect_uri: environment.returnUrl,
      response_type: 'id_token token',
      scope: 'openid profile email roles api',
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      revokeAccessTokenOnSignout: true
    };
  }
}
