import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  AdminLogin(model: any) {
    return this.http.post<any>(this.apiURL + 'User/AdminLogin', model);
  }
}
