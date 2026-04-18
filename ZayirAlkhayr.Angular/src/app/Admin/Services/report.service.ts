import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PagingFilterModel } from '../Models/PagingFilterModel';
import { ApiResponseModel } from '../Models/ApiResponseModel';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

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













  

  GetReportSalesItemStatistics(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportSalesItemStatistics', Model);
  }

  GetReportSalesItemData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportSalesItemData', Model);
  }

  GetReportSalesItemFilter(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportSalesItemFilter', Model);
  }

  GetOrderTypeSalesReport(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'SalesReport/GetOrderTypeSalesReport', Model);
  }

  GetCustomerSalesReport(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'SalesReport/GetCustomerSalesReport', Model);
  }

  GetSalesReportByTimeReport(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'SalesReport/GetSalesReportByTimeReport', Model);
  }
}
