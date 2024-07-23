import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { BenefactorauthService } from './benefactorauth.service';

@Injectable({
  providedIn: 'root'
})
export class BenefactorauthGuard implements CanActivate {

  constructor(private benefactorAuthService: BenefactorauthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.benefactorAuthService.isAuthenticated())
      return true;


    this.benefactorAuthService.loginRedirect();
    return false;
  }

}
