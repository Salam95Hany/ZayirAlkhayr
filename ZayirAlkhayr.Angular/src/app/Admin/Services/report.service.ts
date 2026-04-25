import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PagingFilterModel } from '../Models/PagingFilterModel';
import { ApiResponseModel } from '../Models/ApiResponseModel';
import { ReportExportRequestModel } from '../Models/ReportExportRequestModel';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  CreateGeneralReport(model: ReportExportRequestModel): Observable<HttpResponse<Blob>> {
    return this.http.post(this.apiURL + 'CreateReport/CreateGeneralReport', model, {
      responseType: 'blob',
      observe: 'response'
    });
  }

  // ============================= SalesReport ==============================

  GetReportDailySalesSummary(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportDailySalesSummary', Model);
  }

  ReportDailySalesDetailsData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/ReportDailySalesDetailsData', Model);
  }

  ReportDailySalesDetailsFilter(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/ReportDailySalesDetailsFilter', Model);
  }

  GetReportMonthlySalesSummary(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportMonthlySalesSummary', Model);
  }

  GetReportMonthlySalesDetailsData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportMonthlySalesDetailsData', Model);
  }

  GetReportMonthlySalesDetailsFilter(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportMonthlySalesDetailsFilter', Model);
  }

  // ============================= ItemReport ==============================

   GetReportAllItemsSummary(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportAllItemsSummary', Model);
  }

  GetReportAllItemsData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportAllItemsData', Model);
  }

   GetReportNeverSoldItemsSummary(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportNeverSoldItemsSummary', Model);
  }

  GetReportNeverSoldItemsData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportNeverSoldItemsData', Model);
  }

   GetReportTopSellingItemSummary(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportTopSellingItemSummary', Model);
  }

  GetReportTopSellingItemsData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportTopSellingItemsData', Model);
  }

   GetReportLowestSellingItemsSummary(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportLowestSellingItemsSummary', Model);
  }

  GetReportLowestSellingItemsData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'ItemReport/GetReportLowestSellingItemsData', Model);
  }
}
