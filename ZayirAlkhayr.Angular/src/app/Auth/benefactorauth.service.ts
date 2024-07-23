import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class BenefactorauthService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient, private router: Router) { }

  BeneFactorLogin(Code: number, BeneFactorName: string) {
    return this.http.get<any>(this.apiURL + 'BeneFactor/BeneFactorLogin?Code=' + Code + '&BeneFactorName=' + BeneFactorName);
  }

  isAuthenticated(): boolean {
    let currentUser = JSON.parse(localStorage.getItem('BeneFactorModel'));
    if (!currentUser || this.isLoginExpired(currentUser.loginDate))
      return false;

    return true;
  }

  loginRedirect(): void {
    localStorage.removeItem('BeneFactorModel');
    this.router.navigateByUrl('/benefactor-login');
  }

  isLoginExpired(date: any): boolean {
    let now = new Date().getTime();
    let loginDate = new Date(date);
    const expirationDate = loginDate.setMinutes(loginDate.getMinutes(), 1800);
    return expirationDate < now;
  }


}
