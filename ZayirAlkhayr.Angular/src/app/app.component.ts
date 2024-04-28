import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'ZayirAlkhayr.Angular';

  constructor(private http:HttpClient) {
    
  }

  ngOnInit(): void {
    this.http.get<any>('http://localhost:52792/api/Values').subscribe(data => {
      
    })
  }
}
