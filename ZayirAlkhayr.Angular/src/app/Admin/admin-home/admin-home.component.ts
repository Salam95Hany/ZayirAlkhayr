import { Component, OnInit } from '@angular/core';
import { AdminWebsiteService } from '../Services/admin-website.service';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css']
})
export class AdminHomeComponent implements OnInit {
  UsersData: any[] = [];
  StatisticsData: any;
  TotalCount = 0;

  constructor(private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetStatisticsHome();
    this.GetAllUsers();
  }

  GetStatisticsHome() {
    this.adminService.GetStatisticsHome().subscribe(data => {
      this.StatisticsData = data;
    })
  }

  GetAllUsers() {
    this.adminService.GetAllUsers().subscribe(data => {
      this.UsersData = data.filter(i => i.isActive);
      this.TotalCount = this.UsersData.length;
    })
  }

}
