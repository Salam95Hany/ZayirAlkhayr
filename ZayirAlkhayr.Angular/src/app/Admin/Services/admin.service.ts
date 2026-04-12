import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PagingFilterModel } from '../Models/PagingFilterModel';
import { map } from 'rxjs';
import { ApiResponseModel } from '../Models/ApiResponseModel';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // ============================= Category ==============================

  GetAllCategories(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Category/GetAllCategories', Model);
  }

  AddNewCategory(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Category/AddNewCategory', Model);
  }

  UpdateCategory(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Category/UpdateCategory', Model);
  }

  DeleteCategory(CategoryId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Category/DeleteCategory?CategoryId=' + CategoryId);
  }

  // ============================= Item ==============================

  GetAllProducts(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Item/GetAllProducts', Model);
  }

  GetProductsByCategoryId(CategoryId: number) {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Item/GetProductsByCategoryId?CategoryId=' + CategoryId);
  }

  AddNewProduct(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Item/AddNewProduct', Model);
  }

  UpdateProduct(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Item/UpdateProduct', Model);
  }

  DeleteProduct(ProductId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Item/DeleteProduct?ProductId=' + ProductId);
  }

  // ============================= Order ==============================

  GetAllOrders(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Order/GetAllOrders', Model);
  }

  GetAllOrderFilters(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Order/GetAllOrderFilters', Model);
  }

  GetOrderDetailsByOrderId(OrderId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Order/GetOrderDetailsByOrderId?OrderId=' + OrderId);
  }

  GetOrderWithDetailsByOrderId(OrderId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Order/GetOrderWithDetailsByOrderId?OrderId=' + OrderId);
  }

  AddNewOrder(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Order/AddNewOrder', Model);
  }

  UpdateOrder(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Order/UpdateOrder', Model);
  }

  CancelOrder(VoidReason: string, Action: string, VoidNotes: string, OrderId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Order/CancelOrder?VoidReason=' + VoidReason + '&Action=' + Action + '&VoidNotes=' + VoidNotes + '&OrderId=' + OrderId);
  }

  GetCustomerOrdersHistory(CustomerId: string) {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Order/GetCustomerOrdersHistory?CustomerId=' + CustomerId);
  }

  // ============================= Auth ==============================

  GetAllUsers() {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Auth/GetAllUsers');
  }

  GetStatisticsHome() {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Auth/GetStatisticsHome');
  }

  CreateUser(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Auth/CreateUser', Model);
  }

  EditUser(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Auth/EditUser', Model);
  }

  DeleteUser(UserId: string) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Auth/DeleteUser?UserId=' + UserId);
  }

  GetUserInfoById(UserId: string) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Auth/GetUserInfoById?UserId=' + UserId);
  }

  EditUserProfile(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Auth/EditUserProfile', Model);
  }

  ChangeUserPassword(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Auth/ChangeUserPassword', Model);
  }

  // ============================= Customer ==============================

  GetAllCustomers(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Customer/GetAllCustomers', Model);
  }

  GetCustomerByPhone(PhoneNumber: string) {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Customer/GetCustomerByPhone?PhoneNumber=' + PhoneNumber);
  }

  GetCustomerById(CustomerId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Customer/GetCustomerById?CustomerId=' + CustomerId);
  }

  AddNewCustomer(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Customer/AddNewCustomer', Model);
  }

  UpdateCustomer(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Customer/UpdateCustomer', Model);
  }

  DeleteCustomer(CustomerId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Customer/DeleteCustomer?CustomerId=' + CustomerId);
  }

  // ============================= Dashboard ==============================

  GetDashboardStatistics() {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Dashboard/GetDashboardStatistics');
  }

  GetTop3Orders() {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Dashboard/GetTop3Orders');
  }

  GetTopSellingItemsToday() {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Dashboard/GetTopSellingItemsToday');
  }

  GetTodayOrdersStats() {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Dashboard/GetTodayOrdersStats');
  }

  GetCustomerDeliveryInsights() {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Dashboard/GetCustomerDeliveryInsights');
  }

}
