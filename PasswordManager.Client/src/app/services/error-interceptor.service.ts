import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor {

  constructor(private toastrService: ToastrService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(1),
      catchError((err) => {
        let errorMessage = '';

        if (err.status === 401) {
          // refresh token or navigate to login
          errorMessage = 'Unauthorized';

          // auto logout if 401 response returned from api
          // this.authenticationService.logout();
          // location.reload(true);
        }
        else if(err.status === 403) {
          errorMessage = 'Forbidden';
        }
        else if(err.status === 404) {
          // this.router.navigate(['/not-found']);
          errorMessage = 'Not Found';
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