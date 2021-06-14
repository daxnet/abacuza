import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.scss']
})
export class AuthCallbackComponent implements OnInit {

  constructor(private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute) { }

  async ngOnInit() {
    await this.authService.completeAuthentication();
    this.router.navigate(['/'], { replaceUrl: true });
  }

}
