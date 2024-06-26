import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PasswordsComponent } from './passwords/passwords.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthGuardService } from './services/auth-guard.service';
import { AddEditComponent } from './add-edit/add-edit.component';
import { PasswordGeneratorComponent } from './password-generator/password-generator.component';
import { SettingsComponent } from './settings/settings.component';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { RegistrationSuccessfulComponent } from './registration-successful/registration-successful.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, title: 'Home' },
  { path: 'login', component: LoginComponent, title: 'Login' },
  { path: 'register', component: RegisterComponent, title: 'Register' },
  { path: 'registration-successful', component: RegistrationSuccessfulComponent, title: 'Registration successful' },
  { path: 'email-confirmation/:email/:token', component: EmailConfirmationComponent, title: 'Email Confirmation' },
  { path: 'passwords', component: PasswordsComponent, title: 'Passwords', canActivate: [AuthGuardService] },
  { path: 'passwords/add', component: AddEditComponent, title: 'Add', canActivate: [AuthGuardService] },
  { path: 'passwords/edit/:id', component: AddEditComponent, title: 'Edit', canActivate: [AuthGuardService] },
  { path: 'generator', component: PasswordGeneratorComponent, title: 'Generator', canActivate: [AuthGuardService] },
  { path: 'settings', component: SettingsComponent, title: 'Settings', canActivate: [AuthGuardService] },
  { path: '',   redirectTo: 'home', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent, title: 'Page not found - 404' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
