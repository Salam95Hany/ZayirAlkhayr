import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PDFHeaderSelectedModel, PDFModel } from '../Models/PDFHeaderSelected';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PdfDownloadService {
  PDFHeaderModel: PDFHeaderSelectedModel[] = [];
  apiURL = environment.apiUrl;
  constructor(private http: HttpClient) { }

  ConverHeaderToPDFModel(Arry: any[]) {
    this.PDFHeaderModel = [];
    Arry.forEach((header, index) => {
      let obj: PDFHeaderSelectedModel = {} as PDFHeaderSelectedModel;
      obj.nameEn = header.displayValue;
      obj.nameAr = header.displayName;
      obj.isSelected = false;
      obj.displayOrder = index + 1;
      obj.valueType = header.valueType;
      obj.isAllowSummation = false;
      this.PDFHeaderModel.push(obj);
    });

    return this.PDFHeaderModel;
  }

  DownloadFile(Model: PDFModel, fileName: string, downloadPath: string) {
    return this.http.post(this.apiURL + downloadPath, Model, {
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
