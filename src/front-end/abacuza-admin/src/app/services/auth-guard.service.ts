import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService) {

  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (await this.authService.isAuthenticated()) {
      return true;
    }

    await this.authService.login();
    return false;
  }
  
}
