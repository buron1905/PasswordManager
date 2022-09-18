import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor {

  constructor() { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(1),
      catchError((err) => {
        if (err.status === 401) {
          // refresh token or navigate to login

          // auto logout if 401 response returned from api
          // this.authenticationService.logout();
          // location.reload(true);
        }
        else if(err.status === 403) {
          // this.router.navigate(['/forbidden']);
        }
        else if(err.status === 404) {
          // this.router.navigate(['/not-found']);
        }
        else if(err.status === 500) {
          // this.router.navigate(['/server-error']);
        }
        else if(err.status === 503) {
          // this.router.navigate(['/service-unavailable']);
        }
        else if(err.status === 504) {
          // this.router.navigate(['/gateway-timeout']);
        }
        else if(err.status === 0) {
          // this.router.navigate(['/network-error']);
        }
        else {
          // this.router.navigate(['/error']);
        }
        
        return throwError(() => err);
      })
    )
  }
}
