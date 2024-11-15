import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PagingFilterModel } from '../Models/PagingFilterModel';
import { BeneFactorOrphansModel, OrphansDetails, OrphansDetailsModel } from '../Models/OrphansModel';

@Injectable({
  providedIn: 'root'
})
export class OrphansService {
  apiURL = environment.apiUrl;

  constructor(private http: HttpClient) { }

  GetAllFamilyStatusOrphansType(SearchText: string) {
    return this.http.get<OrphansDetailsModel[]>(this.apiURL + 'Orphans/GetAllFamilyStatusOrphansType?SearchText=' + SearchText);
  }

  GetAllOrphansData(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'Orphans/GetAllOrphansData', PagingFilter);
  }

  GetAllOrphansFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'Orphans/GetAllOrphansFilters', PagingFilter);
  }

  GetOrphanDetailsByFamilyId(FamilyStatusId: number) {
    return this.http.get<OrphansDetails[]>(this.apiURL + 'Orphans/GetOrphanDetailsByFamilyId?FamilyStatusId=' + FamilyStatusId);
  }

  AddNewOrphans(Model: any) {
    return this.http.post<any>(this.apiURL + 'Orphans/AddNewOrphans', Model);
  }

  UpdateOrphans(Model: any) {
    return this.http.post<any>(this.apiURL + 'Orphans/UpdateOrphans', Model);
  }

  DeleteOrphans(OrphansId: number) {
    return this.http.get<any>(this.apiURL + 'Orphans/DeleteOrphans?OrphansId=' + OrphansId);
  }

  AddUpdateBenefactorOrphans(Model: BeneFactorOrphansModel) {
    return this.http.post<any>(this.apiURL + 'Orphans/AddUpdateBenefactorOrphans', Model);
  }

  DeleteBenefactorOrphans(OrphansId: number) {
    return this.http.get<any>(this.apiURL + 'Orphans/DeleteBenefactorOrphans?OrphansId=' + OrphansId);
  }
}
