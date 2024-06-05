import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminWebsiteService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // ============================= SliderImage ==============================

  GetHomeSliderImages() {
    return this.http.get<any[]>(this.apiURL + 'WebsiteHome/GetHomeSliderImages');
  }

  AddNewSliderImage(Model: any) {
    return this.http.post<any>(this.apiURL + 'WebsiteHome/AddNewSliderImage', Model);
  }

  UpdateSliderImage(Model: any) {
    return this.http.post<any>(this.apiURL + 'WebsiteHome/UpdateSliderImage', Model);
  }

  // ============================= Activity ==============================

  GetAllActivities() {
    return this.http.get<any[]>(this.apiURL + 'Activity/GetAllActivities');
  }
}
