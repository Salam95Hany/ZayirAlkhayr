import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FilterModel } from '../Models/FilterModel';
import { PagingFilterModel } from '../Models/PagingFilterModel';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // ============================= AccountsMony ==============================

  GetAllAccountsImportMonyData(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'AccountsMony/GetAllAccountsImportMonyData', PagingFilter);
  }

  GetAllAccountsImportMonyFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'AccountsMony/GetAllAccountsImportMonyFilters', PagingFilter);
  }

  GetAllAccountsExportMonyData(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'AccountsMony/GetAllAccountsExportMonyData', PagingFilter);
  }

  GetAllAccountsExportMonyFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'AccountsMony/GetAllAccountsExportMonyFilters', PagingFilter);
  }

  GetAllImportExportMonyStatistics(PagingFilter: PagingFilterModel) {
    return this.http.post<any>(this.apiURL + 'AccountsMony/GetAllImportExportMonyStatistics', PagingFilter);
  }

  AddNewAccountsImportMony(Model: any) {
    return this.http.post<any>(this.apiURL + 'AccountsMony/AddNewAccountsImportMony', Model);
  }

  AddNewAccountsExportMony(Model: any) {
    return this.http.post<any>(this.apiURL + 'AccountsMony/AddNewAccountsExportMony', Model);
  }

}
