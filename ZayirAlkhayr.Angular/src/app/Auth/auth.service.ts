import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient, private cookieService: CookieService) { }

  AdminLogin(model: any) {
    return this.http.post<any>(this.apiURL + 'User/AdminLogin', model);
  }

  AdminLogout(UserId: string) {
    return this.http.get<any>(this.apiURL + 'User/AdminLogout?UserId=' + UserId);
  }

  CreateSessionId() {
    let sessionId = localStorage.getItem('sessionId');
    if (sessionId)
      return;

    this.http.get<any>(this.apiURL + 'WebsiteHome/CreateSessionId').subscribe(data => {
      const sessionId = data.sessionId;
      localStorage.setItem('sessionId', sessionId);
    });
  }

  isInRole(roles: string[]): boolean {
    let userModel = JSON.parse(localStorage.getItem('UserModel'));
    if (!userModel)
      return false;

    let ckeckRole = roles.some(i => i == userModel?.role);
    return ckeckRole;
  }


}
