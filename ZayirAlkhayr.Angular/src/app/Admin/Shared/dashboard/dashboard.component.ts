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
  readonly stockAlerts = [
    { name: 'سكر', level: 'منخفض', tone: 'low', color: '#dc2626' },
    { name: 'قهوة', level: 'منخفض', tone: 'low', color: '#ea580c' },
    { name: 'حليب', level: 'منخفض', tone: 'low', color: '#d97706' },
    { name: 'شاي', level: 'نفد', tone: 'out', color: '#334155' },
    { name: 'جبنة', level: 'نفد', tone: 'out', color: '#0f172a' }
  ];
  readonly lowStockAlerts = this.stockAlerts.filter(item => item.tone === 'low');
  readonly outOfStockAlerts = this.stockAlerts.filter(item => item.tone === 'out');

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

  normalizePercent(value: number | null | undefined): number {
    const numericValue = Number(value ?? 0);

    if (Number.isNaN(numericValue)) {
      return 0;
    }

    return Math.min(100, Math.max(0, Math.round(numericValue)));
  }

  getTrendClass(trend: string | null | undefined): string {
    return (trend ?? '').toLowerCase() === 'down' ? 'dashboard-trend--down' : 'dashboard-trend--up';
  }

  getTrendIcon(trend: string | null | undefined): string {
    return (trend ?? '').toLowerCase() === 'down' ? 'uil-arrow-down-right' : 'uil-arrow-up-right';
  }

  getOrderStatusLabel(status: number | null | undefined): string {
    return status === 2 ? 'ملغي' : 'منتهي';
  }

  getOrderStatusClass(status: number | null | undefined): string {
    return status === 2 ? 'dashboard-status-chip--cancelled' : 'dashboard-status-chip--completed';
  }

  getOrderDotClass(status: number | null | undefined): string {
    return status === 2 ? 'dashboard-order-dot--cancelled' : 'dashboard-order-dot--completed';
  }

  getTopSellingWidth(quantity: number | null | undefined): number {
    const maxQuantity = Math.max(...this.TopSellingItems.map(item => Number(item?.totalQuantity) || 0), 0);

    if (!maxQuantity) {
      return 0;
    }

    return this.normalizePercent((Number(quantity ?? 0) / maxQuantity) * 100);
  }

  getDonutDasharray(percent: number | null | undefined): string {
    const circumference = 264;
    const safePercent = this.normalizePercent(percent);

    return `${(safePercent / 100) * circumference} ${circumference}`;
  }

  getDonutDashoffset(percent: number | null | undefined): number {
    const circumference = 264;
    const safePercent = this.normalizePercent(percent);

    return -((safePercent / 100) * circumference);
  }
}
