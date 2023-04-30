import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

export const maxRetries = 0;
export const delayMs = 2000;


@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor {


  constructor(private toastrService: ToastrService, private router: Router, private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(maxRetries),
      catchError((err) => {
        let errorMessage = '';

        if (err.status === 401) {
          // refresh token or navigate to login
          errorMessage = 'Unauthorized';

          if (this.router.url !== '/login') {
            this.router.navigate(['/login']);

            this.authService.logout(false);
          }
          else {
            this.authService.logout(true);
          }
        }
        else if(err.status === 403) {
          errorMessage = 'Forbidden';
        }
        else if(err.status === 404) {
          errorMessage = 'Not Found';
          this.router.navigate(['/page-not-found']);
        }
        else if(err.status === 500) {
          errorMessage = 'Server Error';
        }
        else if(err.status === 503) {
          errorMessage = 'Service Unavailable';
        }
        else if(err.status === 504) {
          errorMessage = 'Gateway Timeout';
        }
        else if(err.status === 0) {
          errorMessage = 'Network Error';
        }
        else {
          errorMessage = 'Error';
        }

        if (errorMessage && errorMessage.trim())
          this.toastrService.error(errorMessage);
        return throwError(() => err);
      })
    )
  }
}
