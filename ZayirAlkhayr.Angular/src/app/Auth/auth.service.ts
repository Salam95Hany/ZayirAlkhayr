import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient, private router: Router) { }

  AdminLogin(model: any) {
    return this.http.post<any>(this.apiURL + 'Auth/AdminLogin', model);
  }

  AdminLogout(UserId: string) {
    return this.http.get<any>(this.apiURL + 'Auth/AdminLogout?UserId=' + UserId);
  }

  isAuthenticated(): boolean {
    const currentUser = this.getStoredUser();
    if (!currentUser || this.isTokenExpired())
      return false;

    return true;
  }

  isTokenExpired(): boolean {
    try {
      const access_token = this.getStoredUser()?.token;
      if (!access_token)
        return true;

      const decode = jwtDecode(access_token);
      if (!decode.exp)
        return true;

      const expirationDate = decode.exp * 1000;
      const now = new Date().getTime();
      return expirationDate < now;
    } catch {
      return true;
    }
  }

  isInRole(roles: string[]): boolean {
    const userModel = this.getStoredUser();
    if (!userModel)
      return false;

    const ckeckRole = roles.some(i => i == userModel?.role);
    return ckeckRole;
  }

  loginRedirect(): void {
    localStorage.removeItem('UserModel');
    this.router.navigateByUrl('/login');
  }

  private getStoredUser(): any | null {
    try {
      const userModel = localStorage.getItem('UserModel');
      return userModel ? JSON.parse(userModel) : null;
    } catch {
      return null;
    }
  }

}
