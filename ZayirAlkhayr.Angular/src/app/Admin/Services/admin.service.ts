import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { FilterModel } from '../Models/FilterModel';
import { PagingFilterModel } from '../Models/PagingFilterModel';
import { map } from 'rxjs';

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

  UpdateAccountsImportMony(Model: any) {
    return this.http.post<any>(this.apiURL + 'AccountsMony/UpdateAccountsImportMony', Model);
  }

  DeleteAccountsImportMony(AccountId: number) {
    return this.http.get<any>(this.apiURL + 'AccountsMony/DeleteAccountsImportMony?AccountId=' + AccountId);
  }

  AddNewAccountsExportMony(Model: any) {
    return this.http.post<any>(this.apiURL + 'AccountsMony/AddNewAccountsExportMony', Model);
  }

  UpdateAccountsExportMony(Model: any) {
    return this.http.post<any>(this.apiURL + 'AccountsMony/UpdateAccountsExportMony', Model);
  }

  DeleteAccountsExportMony(AccountId: number) {
    return this.http.get<any>(this.apiURL + 'AccountsMony/DeleteAccountsExportMony?AccountId=' + AccountId);
  }

  // ============================= DbBackup ==============================

  DownloadBackupFile(fileName: string) {
    return this.http.get(this.apiURL + 'DbBackup/SaveDbBackupFile', {
      responseType: 'blob',
      observe: 'response'
    }).pipe(
      map((response: any) => {
        const downloadLink = document.createElement('a');
        downloadLink.href = URL.createObjectURL(new Blob([response.body], { type: response.body.type }));
        downloadLink.download = fileName + '.bak';
        downloadLink.click();
      })
    );
  }

  DownloadZipFile(Folder: string, fileName: string) {
    return this.http.get(this.apiURL + 'DbBackup/DownloadImagesFolder?Folder=' + Folder, {
      responseType: 'blob',
      observe: 'response'
    }).pipe(
      map((response: any) => {
        const downloadLink = document.createElement('a');
        downloadLink.href = URL.createObjectURL(new Blob([response.body], { type: response.body.type }));
        downloadLink.download = fileName + '.zip';
        downloadLink.click();
      })
    );
  }

  // ============================= GeneralTasks ==============================

  GetAllGeneralTasksData(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'GeneralTasks/GetAllGeneralTasksData', PagingFilter);
  }

  GetAllGeneralTasksFilter(PagingFilter: PagingFilterModel) {
    return this.http.post<any[]>(this.apiURL + 'GeneralTasks/GetAllGeneralTasksFilter', PagingFilter);
  }

  GetAllUserTasks(UserId: string) {
    return this.http.get<any[]>(this.apiURL + 'GeneralTasks/GetAllUserTasks?UserId=' + UserId);
  }

  AddNewGeneralTask(Model: any) {
    return this.http.post<any>(this.apiURL + 'GeneralTasks/AddNewGeneralTask', Model);
  }

  UpdateGeneralTask(Model: any) {
    return this.http.post<any>(this.apiURL + 'GeneralTasks/UpdateGeneralTask', Model);
  }

  DeleteGeneralTask(TaskId: number) {
    return this.http.get<any>(this.apiURL + 'GeneralTasks/DeleteGeneralTask?TaskId=' + TaskId);
  }

  ConvertTaskStatus(TaskId: number, StatusId: number) {
    return this.http.get<any>(this.apiURL + 'GeneralTasks/ConvertTaskStatus?TaskId=' + TaskId + '&StatusId=' + StatusId);
  }

}
