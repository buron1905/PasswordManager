import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthService } from './services/auth.service';
import { AuthGuardService } from './services/auth-guard.service';
//import { TokenInterceptorService } from './services/token-interceptor.service';
import { ErrorInterceptorService } from './services/error-interceptor.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { PasswordService } from './services/password.service';
import { CreatePasswordComponent } from './create-password/create-password.component';
import { JwtModule } from '@auth0/angular-jwt';
import { HomeComponent } from './home/home.component';
import { PasswordsComponent } from './passwords/passwords.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { DetailsPasswordComponent } from './details-password/details-password.component';
import { UpdatePasswordComponent } from './update-password/update-password.component';
import { TitleStrategy } from '@angular/router';
import { AppTitleSuffixService } from './services/app-title-suffix-service';

export function tokenGetter() { 
  return localStorage.getItem("token"); 
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    CreatePasswordComponent,
    HomeComponent,
    PasswordsComponent,
    NavigationBarComponent,
    PageNotFoundComponent,
    DetailsPasswordComponent,
    UpdatePasswordComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    }),
  ],
  providers: [
    AuthService,
    AuthGuardService,
    PasswordService,
    {
      provide: HTTP_INTERCEPTORS ,
      useClass: ErrorInterceptorService,
      multi: true
    },
    {
      provide: TitleStrategy,
      useClass: AppTitleSuffixService
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
