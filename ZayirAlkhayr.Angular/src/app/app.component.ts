import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ZayirAlkhayr.Angular';

  constructor(private http: HttpClient) {

  }

  ExportExcelFile() {
    return this.http.get('http://localhost:52792/api/WebSite/ExportFile', {
      responseType: 'blob',
      observe: 'response'
    }).pipe(
      map((response: any) => {
        debugger;
        const downloadLink = document.createElement('a');
        downloadLink.href = URL.createObjectURL(new Blob([response.body], { type: response.body.type }));
        downloadLink.download = "TestTest.xlsx";
        downloadLink.click();
      })
    ).subscribe();;
  }


}
