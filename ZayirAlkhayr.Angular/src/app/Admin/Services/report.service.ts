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

  GetSalesReportStatistics(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetSalesReportStatistics', Model);
  }

  GetReportSalesData(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportSalesData', Model);
  }

  GetReportSalesFilter(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'SalesReport/GetReportSalesFilter', Model);
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
