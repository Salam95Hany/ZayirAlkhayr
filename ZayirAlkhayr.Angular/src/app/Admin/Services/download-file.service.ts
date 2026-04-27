import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SearchReportModel } from '../Models/SearchReportModel';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DownloadFileService {
apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  DownloadFile(Model: SearchReportModel, fileName: string) {
    return this.http.post(this.apiURL + 'CreateReport/CreateGeneralReport', Model, {
      responseType: 'blob',
      observe: 'response'
    }).pipe(
      map((response: any) => {
        const downloadLink = document.createElement('a');
        downloadLink.href = URL.createObjectURL(new Blob([response.body], { type: response.body.type }));
        downloadLink.download = fileName;
        downloadLink.click();
      })
    );
  }
}
