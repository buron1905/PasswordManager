import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthService } from './services/auth.service';
import { EncryptionService } from './services/encryption.service';
import { AuthGuardService } from './services/auth-guard.service';
import { ErrorInterceptorService } from './services/error-interceptor.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { PasswordService } from './services/password.service';
import { JwtModule } from '@auth0/angular-jwt';
import { HomeComponent } from './home/home.component';
import { PasswordsComponent } from './passwords/passwords.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { TitleStrategy } from '@angular/router';
import { AppTitleSuffixService } from './services/app-title-suffix-service';
import { AddEditComponent } from './add-edit/add-edit.component';
import { NgxLoadingModule } from 'ngx-loading';
import { ClipboardModule } from 'ngx-clipboard';
import { CallbackPipe } from './utils/callback.pipe';
import { PasswordGeneratorComponent } from './password-generator/password-generator.component';
import { ModalDeletePasswordComponent } from './modal-delete-password/modal-delete-password.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalGeneratePasswordComponent } from './modal-generate-password/modal-generate-password.component';
import { ModalTfaLoginComponent } from './modal-tfa-login/modal-tfa-login.component';
import { SettingsComponent } from './settings/settings.component';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { RegistrationSuccessfulComponent } from './registration-successful/registration-successful.component';

export function tokenGetter() { 
  return localStorage.getItem("token"); 
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    PasswordsComponent,
    NavigationBarComponent,
    PageNotFoundComponent,
    AddEditComponent,
    CallbackPipe,
    PasswordGeneratorComponent,
    ModalDeletePasswordComponent,
    ModalGeneratePasswordComponent,
    ModalTfaLoginComponent,
    SettingsComponent,
    EmailConfirmationComponent,
    RegistrationSuccessfulComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ClipboardModule,
    ToastrModule.forRoot(),
    NgxLoadingModule.forRoot({}),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    }),
    NgbModule,
  ],
  providers: [
    AuthService,
    AuthGuardService,
    PasswordService,
    EncryptionService,
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
  bootstrap: [AppComponent],
  entryComponents: [ModalDeletePasswordComponent]
})
export class AppModule { }
