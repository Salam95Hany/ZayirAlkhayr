import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PdfDownloadService {
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  DownloadFile(fileName: string, downloadPath: string) {
    return this.http.post(this.apiURL + downloadPath, {
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
