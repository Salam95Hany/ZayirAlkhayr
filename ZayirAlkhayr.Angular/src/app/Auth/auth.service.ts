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

  CreateSessionId() {
    let sessionId = this.cookieService.get('sessionId');
    if (sessionId)
      return;

    this.http.get<any>(this.apiURL + 'WebsiteHome/CreateSessionId').subscribe(data => {
      const sessionId = data.sessionId;
      const expirationDate = new Date();
      expirationDate.setDate(expirationDate.getDate() + 9999);
      this.cookieService.set('sessionId', sessionId, expirationDate);
    });
  }


}
