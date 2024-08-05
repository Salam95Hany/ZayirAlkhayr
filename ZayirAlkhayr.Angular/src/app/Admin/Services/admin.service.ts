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

  AddNewAccountsExportMony(Model: any) {
    return this.http.post<any>(this.apiURL + 'AccountsMony/AddNewAccountsExportMony', Model);
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

}
