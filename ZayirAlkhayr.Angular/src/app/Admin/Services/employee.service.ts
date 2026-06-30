import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PagingFilterModel } from '../Models/PagingFilterModel';
import { ApiResponseModel } from '../Models/ApiResponseModel';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // ============================= Employee ==============================

  GetAllEmployees(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Employee/GetAllEmployees', Model);
  }

  GetAllEmployeeFilters(Model: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.apiURL + 'Employee/GetAllEmployeeFilters', Model);
  }

  AddNewEmployee(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Employee/AddNewEmployee', Model);
  }

  UpdateEmployee(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Employee/UpdateEmployee', Model);
  }

  DeleteEmployee(EmployeeId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Employee/DeleteEmployee?EmployeeId=' + EmployeeId);
  }

  // ============================= JobTitle ==============================

  GetAllJobTitle() {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Employee/GetAllJobTitle');
  }

  AddNewJobTitle(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Employee/AddNewJobTitle', Model);
  }

  UpdateJobTitle(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Employee/UpdateJobTitle', Model);
  }

  DeleteJobTitle(JobTitleId: number) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Employee/DeleteJobTitle?JobTitleId=' + JobTitleId);
  }

  // ============================= EmployeeSalary ==============================

  GetEmployeeSalary(SalaryDate: any) {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Employee/GetEmployeeSalary?SalaryDate=' + SalaryDate);
  }

  CheckMonthEmployeePaid(SalaryDate: any) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Employee/CheckMonthEmployeePaid?SalaryDate=' + SalaryDate);
  }

  CreateMonthEmployeeSalary(SalaryDate: any, InsertUser: any) {
    return this.http.get<ApiResponseModel<any[]>>(this.apiURL + 'Employee/CreateMonthEmployeeSalary?SalaryDate=' + SalaryDate + '&InsertUser=' + InsertUser);
  }

  PaidEmployeeSalary(Model: any, SalaryDate: any, UpdateUser: any) {
    return this.http.post<ApiResponseModel<any>>(this.apiURL + 'Employee/PaidEmployeeSalary?SalaryDate=' + SalaryDate + '&UpdateUser=' + UpdateUser, Model);
  }

  PayrollTransferToExpenses(TotalAmount: any, SalaryDate: any, InsertUser: any) {
    return this.http.get<ApiResponseModel<any>>(this.apiURL + 'Employee/PayrollTransferToExpenses?TotalAmount=' + TotalAmount + '&SalaryDate=' + SalaryDate + '&InsertUser=' + InsertUser);
  }
}
