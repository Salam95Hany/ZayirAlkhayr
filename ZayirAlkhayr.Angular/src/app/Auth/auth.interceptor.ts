import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRedirectingToLogin = false;

  constructor(private authService: AuthService, private router: Router) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessToken = this.authService.getAccessToken();
    const authRequest = accessToken
      ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`
        }
      })
      : req;

    return next.handle(authRequest).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.redirectToLogin();
        }

        return throwError(() => error);
      })
    );
  }

  private redirectToLogin(): void {
    if (this.isRedirectingToLogin)
      return;

    this.isRedirectingToLogin = true;
    const currentUrl = this.authService.resolveReturnUrl(this.router.url);
    this.authService.loginRedirect(currentUrl).then(
      () => this.isRedirectingToLogin = false,
      () => this.isRedirectingToLogin = false
    );
  }
}
