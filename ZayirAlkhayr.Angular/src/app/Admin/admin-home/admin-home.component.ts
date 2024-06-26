import { Component, OnInit } from '@angular/core';
import { AdminWebsiteService } from '../Services/admin-website.service';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css']
})
export class AdminHomeComponent implements OnInit {
  StatisticsData: any;
  constructor(private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetStatisticsHome();
  }

  GetStatisticsHome() {
    this.adminService.GetStatisticsHome().subscribe(data => {
      this.StatisticsData = data;
    })
  }

}
