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
import { UpdatePasswordComponent } from './update-password/update-password.component';
import { AddEditComponent } from './add-edit/add-edit.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, title: 'Home', canActivate: [AuthGuardService] },
  { path: 'login', component: LoginComponent, title: 'Login' },
  { path: 'register', component: RegisterComponent, title: 'Register' },
  { path: 'passwords', component: PasswordsComponent, title: 'Passwords', canActivate: [AuthGuardService] },
  { path: 'passwords/new', component: CreatePasswordComponent, title: 'New', canActivate: [AuthGuardService] },

  { path: 'passwords/add', component: AddEditComponent, title: 'Add', canActivate: [AuthGuardService] },
  { path: 'passwords/edit/:id', component: AddEditComponent, title: 'Edit', canActivate: [AuthGuardService] },

  { path: 'passwords/details/:id', component: DetailsPasswordComponent, title: 'Details', canActivate: [AuthGuardService] },
  { path: 'passwords/update/:id', component: UpdatePasswordComponent, title: 'Edit',canActivate: [AuthGuardService] },
  { path: '',   redirectTo: 'home', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent, title: 'Page not found - 404' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
