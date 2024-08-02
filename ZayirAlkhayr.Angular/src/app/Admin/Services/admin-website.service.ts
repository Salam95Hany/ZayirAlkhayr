import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FilterModel } from '../Models/FilterModel';
import { PagingFilterModel } from '../Models/PagingFilterModel';

@Injectable({
  providedIn: 'root'
})
export class AdminWebsiteService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // ============================= SliderImage ==============================

  GetHomeSliderImages(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'WebsiteHome/GetHomeSliderImages', PagingFilter);
  }

  GetAllWebPagesFilters(PageName: string) {
    return this.http.get<FilterModel[]>(this.apiURL + 'WebsiteHome/GetAllWebPagesFilters?PageName=' + PageName);
  }

  GetPagesAutoSearch(SearchText: string) {
    return this.http.get<any[]>(this.apiURL + 'WebsiteHome/GetPagesAutoSearch?SearchText=' + SearchText);
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

  GetAllActivities(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'Activity/GetAllActivities', PagingFilter);
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

  GetAllPhotos(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'Photo/GetAllPhotos', PagingFilter);
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

  GetAllEvents(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'Event/GetAllEvents', PagingFilter);
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

  GetStatisticsHome() {
    return this.http.get<any>(this.apiURL + 'User/GetStatisticsHome');
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

  // ============================= BeneFactor ==============================

  GetAllBeneFactorData(PagingFilter: PagingFilterModel) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/GetAllBeneFactorData', PagingFilter);
  }

  GetAllBeneFactorFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'BeneFactor/GetAllBeneFactorFilters', PagingFilter);
  }

  GetAllBeneFactorParentById(BeneFactorId: number) {
    return this.http.get<any[]>(this.apiURL + 'BeneFactor/GetAllBeneFactorParentById?BeneFactorId=' + BeneFactorId);
  }

  GetAllBeneFactorTypes(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'BeneFactor/GetAllBeneFactorTypes', PagingFilter);
  }

  GetAllBeneFactorDetails(PagingFilter: PagingFilterModel, BeneFactorId: number) {
    return this.http.post<any[]>(this.apiURL + 'BeneFactor/GetAllBeneFactorDetails?BeneFactorId=' + BeneFactorId, PagingFilter);
  }

  GetAllBeneFactorCashDetails(BeneFactorId: number, ParentId: any) {
    return this.http.get<any[]>(this.apiURL + 'BeneFactor/GetAllBeneFactorCashDetails?BeneFactorId=' + BeneFactorId + '&ParentId=' + ParentId);
  }

  GetBeneFactorDetailsByBeneFactorId(BeneFactorId: number, BeneFactorTypeId: number) {
    return this.http.get<any[]>(this.apiURL + 'BeneFactor/GetBeneFactorDetailsByBeneFactorId?BeneFactorId=' + BeneFactorId + '&BeneFactorTypeId=' + BeneFactorTypeId);
  }

  GetBeneFactorDetailsStatistics(BeneFactorId: number) {
    return this.http.get<any[]>(this.apiURL + 'BeneFactor/GetBeneFactorDetailsStatistics?BeneFactorId=' + BeneFactorId);
  }

  GetBeneFactorTypeByIds(BeneFactorTypeIds: number[]) {
    return this.http.post<any[]>(this.apiURL + 'BeneFactor/GetBeneFactorTypeByIds', BeneFactorTypeIds);
  }

  GetBeneFactorNotes(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'BeneFactor/GetBeneFactorNotes', PagingFilter);
  }

  GetBeneFactorWelcomeMessage() {
    return this.http.get<any>(this.apiURL + 'BeneFactor/GetBeneFactorWelcomeMessage');
  }

  GetAllBeneFactorNationalities(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'BeneFactor/GetAllBeneFactorNationalities', PagingFilter);
  }

  AddNewBeneFactor(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/AddNewBeneFactor', Model);
  }

  AddNewBeneFactorValues(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/AddNewBeneFactorValues', Model);
  }

  AddNewBeneFactorType(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/AddNewBeneFactorType', Model);
  }

  AddNewBeneFactorDetails(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/AddNewBeneFactorDetails', Model);
  }

  AddNewBeneFactorNotes(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/AddNewBeneFactorNotes', Model);
  }

  AddNewBeneFactorWelcomeMessage(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/AddNewBeneFactorWelcomeMessage', Model);
  }

  AddNewBeneFactorNationality(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/AddNewBeneFactorNationality', Model);
  }

  UpdateBeneFactor(Model: any) {
    return this.http.post<any>(this.apiURL + 'BeneFactor/UpdateBeneFactor', Model);
  }

  DeleteBeneFactor(BeneFactorId: number) {
    return this.http.get<any>(this.apiURL + 'BeneFactor/DeleteBeneFactor?BeneFactorId=' + BeneFactorId);
  }
}
