import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpAuthInterceptorService implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authHeader = this.authService.authorizationHeaderValue;
    req = req.clone({
      setHeaders: {
        'Content-Type': "application/json",
        'Authorization': authHeader
      }
    });

    return next.handle(req);
  }
}
