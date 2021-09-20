import { Component, OnInit } from '@angular/core';
import { AuthComponentBase } from 'src/app/classes/auth-component-base';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-footer-bar',
  templateUrl: './footer-bar.component.html',
  styleUrls: ['./footer-bar.component.scss']
})
export class FooterBarComponent extends AuthComponentBase {

  constructor(authService: AuthService) {
    super(authService);
   }

}
