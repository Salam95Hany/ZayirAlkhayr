import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../Services/admin.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  StatisticsData: any;
  TopSellingItems: any[] = [];
  TopOrderData: any[] = [];
  TodayOrdersStats: any;
  CustomerDelivery: any;
  constructor(private adminService: AdminService) {

  }

  ngOnInit(): void {
    this.GetDashboardStatistics();
    this.GetTop3Orders();
    this.GetTopSellingItemsToday();
    this.GetTodayOrdersStats();
    this.GetCustomerDeliveryInsights();
  }

  GetDashboardStatistics() {
    this.adminService.GetDashboardStatistics().subscribe(data => {
      this.StatisticsData = data?.results[0];
    });
  }

  GetTop3Orders() {
    this.adminService.GetTop3Orders().subscribe(data => {
      this.TopOrderData = data.results;
    });
  }

  GetTopSellingItemsToday() {
    this.adminService.GetTopSellingItemsToday().subscribe(data => {
      this.TopSellingItems = data.results;
    });
  }

  GetTodayOrdersStats() {
    this.adminService.GetTodayOrdersStats().subscribe(data => {
      this.TodayOrdersStats = data.results;
    });
  }

  GetCustomerDeliveryInsights() {
    this.adminService.GetCustomerDeliveryInsights().subscribe(data => {
      this.CustomerDelivery = data.results[0];
    });
  }
}
