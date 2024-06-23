import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UploadFileModel } from '../Models/FileModel';

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

  DeleteSliderImage(SliderImageId: number) {
    return this.http.get<any>(this.apiURL + 'WebsiteHome/DeleteSliderImage?SliderImageId=' + SliderImageId);
  }

  // ============================= Activity ==============================

  GetAllActivities() {
    return this.http.get<any[]>(this.apiURL + 'Activity/GetAllActivities');
  }

  GetActivitySliderImagesById(ActivityId: number) {
    return this.http.get<any[]>(this.apiURL + 'Activity/GetActivitySliderImagesById?ActivityId=' + ActivityId);
  }

  GetActivityWithSliderImagesById(ActivityId: number) {
    return this.http.get<any>(this.apiURL + 'Activity/GetActivityWithSliderImagesById?ActivityId=' + ActivityId);
  }

  AddNewActivity(Model: any) {
    return this.http.post<any>(this.apiURL + 'Activity/AddNewActivity', Model);
  }

  AddActivitySliderImage(Model: any) {
    return this.http.post<any>(this.apiURL + 'Activity/AddActivitySliderImage', Model);
  }

  UpdateActivity(Model: any) {
    return this.http.post<any>(this.apiURL + 'Activity/UpdateActivity', Model);
  }

  DeleteActivity(ActivityId: number) {
    return this.http.get<any>(this.apiURL + 'Activity/DeleteActivity?ActivityId=' + ActivityId);
  }

  // ============================= Photos ==============================

  GetAllPhotos() {
    return this.http.get<any[]>(this.apiURL + 'Photo/GetAllPhotos');
  }

  GetPhotoDetails(PhotoId: number) {
    return this.http.get<any[]>(this.apiURL + 'Photo/GetPhotoDetails?PhotoId=' + PhotoId);
  }

  GetPhotoWithDetailsById(PhotoId: number) {
    return this.http.get<any>(this.apiURL + 'Photo/GetPhotoWithDetailsById?PhotoId=' + PhotoId);
  }

  AddNewPhoto(Model: any) {
    return this.http.post<any>(this.apiURL + 'Photo/AddNewPhoto', Model);
  }

  AddPhotoDetailsImage(Model: any) {
    return this.http.post<any>(this.apiURL + 'Photo/AddPhotoDetailsImage', Model);
  }

  UpdatePhoto(Model: any) {
    return this.http.post<any>(this.apiURL + 'Photo/UpdatePhoto', Model);
  }

  DeletePhoto(PhotoId: number) {
    return this.http.get<any>(this.apiURL + 'Photo/DeletePhoto?PhotoId=' + PhotoId);
  }

  // ============================= Events ==============================

  GetAllEvents() {
    return this.http.get<any[]>(this.apiURL + 'Event/GetAllEvents');
  }

  GetAllWebSiteEvents() {
    return this.http.get<any[]>(this.apiURL + 'Event/GetAllWebSiteEvents');
  }

  GetEventSliderImagesById(EventId: number) {
    return this.http.get<any>(this.apiURL + 'Event/GetEventSliderImagesById?EventId=' + EventId);
  }

  AddNewEvent(Model: any) {
    return this.http.post<any>(this.apiURL + 'Event/AddNewEvent', Model);
  }

  AddEventSliderImage(Model: any) {
    return this.http.post<any>(this.apiURL + 'Event/AddEventSliderImage', Model);
  }

  UpdateEvent(Model: any) {
    return this.http.post<any>(this.apiURL + 'Event/UpdateEvent', Model);
  }

  DeleteEvent(EventId: number) {
    return this.http.get<any>(this.apiURL + 'Event/DeleteEvent?EventId=' + EventId);
  }

  // ============================= Users ==============================

  GetAllUsers() {
    return this.http.get<any[]>(this.apiURL + 'User/GetAllUsers');
  }

  CreateUser(Model: any) {
    return this.http.post<any>(this.apiURL + 'User/CreateUser', Model);
  }

  EditUser(Model: any) {
    return this.http.post<any>(this.apiURL + 'User/EditUser', Model);
  }

  DeleteUser(UserId: string) {
    return this.http.get<any>(this.apiURL + 'User/DeleteUser?UserId=' + UserId);
  }
}
