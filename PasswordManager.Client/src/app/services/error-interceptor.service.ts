import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

export const maxRetries = 1;
export const delayMs = 2000;


@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor {


  constructor(private toastrService: ToastrService, private router: Router) { }

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

            // auto logout if 401 response returned from api
            //this.authenticationService.logout();
            // location.reload(true);
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
          errorMessage = 'Unknown Error';
        }
        
        this.toastrService.error(errorMessage);
        return throwError(() => err);
      })
    )
  }
}
