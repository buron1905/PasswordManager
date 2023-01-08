import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreatePasswordComponent } from './create-password/create-password.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PasswordsComponent } from './passwords/passwords.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthGuardService } from './services/auth-guard.service';
import { DetailsPasswordComponent } from './details-password/details-password.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [AuthGuardService] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'passwords', component: PasswordsComponent, canActivate: [AuthGuardService] },
  { path: 'passwords/new', component: CreatePasswordComponent, canActivate: [AuthGuardService] },
  { path: 'passwords/details/:id', component: DetailsPasswordComponent, canActivate: [AuthGuardService] },
  { path: '',   redirectTo: 'home', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
