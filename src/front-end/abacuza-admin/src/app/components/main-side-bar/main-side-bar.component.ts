import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthComponentBase } from 'src/app/classes/auth-component-base';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-main-side-bar',
  templateUrl: './main-side-bar.component.html',
  styleUrls: ['./main-side-bar.component.scss']
})
export class MainSideBarComponent extends AuthComponentBase {

  constructor (authService: AuthService) {
    super(authService);
  }

}
