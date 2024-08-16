import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PagingFilterModel } from '../Models/PagingFilterModel';

@Injectable({
  providedIn: 'root'
})
export class GeneralStatusService {
  apiURL = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // ============================= FamilyNeeds ==============================

  GetAllFamilyNeedTypesData(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'FamilyNeeds/GetAllFamilyNeedTypesData', PagingFilter);
  }

  GetAllFamilyNeedTypesFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'FamilyNeeds/GetAllFamilyNeedTypesFilters', PagingFilter);
  }

  GetAllFamilyNeedCategoriesData(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'FamilyNeeds/GetAllFamilyNeedCategoriesData', PagingFilter);
  }

  GetAllFamilyNeedCategoriesFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'FamilyNeeds/GetAllFamilyNeedCategoriesFilters', PagingFilter);
  }

  GetAllFamilyNeedCategories() {
    return this.http.get<any[]>(this.apiURL + 'FamilyNeeds/GetAllFamilyNeedCategories');
  }

  AddNewFamilyNeedType(Model: any) {
    return this.http.post<any>(this.apiURL + 'FamilyNeeds/AddNewFamilyNeedType', Model);
  }

  AddNewFamilyNeedCategory(Model: any) {
    return this.http.post<any>(this.apiURL + 'FamilyNeeds/AddNewFamilyNeedCategory', Model);
  }

  UpdateFamilyNeedType(Model: any) {
    return this.http.post<any>(this.apiURL + 'FamilyNeeds/UpdateFamilyNeedType', Model);
  }

  UpdateFamilyNeedCategory(Model: any) {
    return this.http.post<any>(this.apiURL + 'FamilyNeeds/UpdateFamilyNeedCategory', Model);
  }

  DeleteFamilyNeedType(NeedTypeId: number) {
    return this.http.get<any>(this.apiURL + 'FamilyNeeds/DeleteFamilyNeedType?NeedTypeId=' + NeedTypeId);
  }

  DeleteFamilyNeedCategory(CategoryId: number) {
    return this.http.get<any>(this.apiURL + 'FamilyNeeds/DeleteFamilyNeedCategory?CategoryId=' + CategoryId);
  }

  // ============================= FamilyNationality ==============================

  GetAllFamilyNationalitiesData(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'FamilyNationality/GetAllFamilyNationalitiesData', PagingFilter);
  }

  GetAllFamilyNationalitiesFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'FamilyNationality/GetAllFamilyNationalitiesFilters', PagingFilter);
  }

  AddNewFamilyNationality(Model: any) {
    return this.http.post<any>(this.apiURL + 'FamilyNationality/AddNewFamilyNationality', Model);
  }

  UpdateFamilyNationality(Model: any) {
    return this.http.post<any>(this.apiURL + 'FamilyNationality/UpdateFamilyNationality', Model);
  }

  DeleteFamilyNationality(NationalityId: number) {
    return this.http.get<any>(this.apiURL + 'FamilyNationality/DeleteFamilyNationality?NationalityId=' + NationalityId);
  }
}
