import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isAuthenticated()) {
      const allowedRoles: string[] = route.data["roles"];
      if (allowedRoles && allowedRoles.length > 0) {
        if (this.authService.isInRole(allowedRoles)) {
          return true;
        } else {
          this.router.navigateByUrl('/not-authorized');
          return false;
        }
      }
      return true;
    }

    this.authService.loginRedirect();
    return false;
  }

}
