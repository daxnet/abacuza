import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { MainSideBarComponent } from './components/main-side-bar/main-side-bar.component';
import { FooterBarComponent } from './components/footer-bar/footer-bar.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { InstalledPluginsComponent } from './pages/installed-plugins/installed-plugins.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpAuthInterceptorService } from './services/http-auth-interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    MainSideBarComponent,
    FooterBarComponent,
    DashboardComponent,
    NotFoundComponent,
    AuthCallbackComponent,
    InstalledPluginsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpAuthInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
