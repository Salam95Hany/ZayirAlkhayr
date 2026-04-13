import { Component, OnInit } from '@angular/core';

type SalesQuickRange = 'today' | 'week' | 'month' | 'quarter' | 'all' | 'custom';
type SalesStatus = 'completed' | 'pending' | 'cancelled';
type SalesOrderType = 'takeaway' | 'delivery';
type SalesPaymentMethod = 'cash' | 'card' | 'wallet';

interface SalesReportRow {
  orderId: number;
  orderNumber: number;
  customerName: string;
  cashierName: string;
  orderType: SalesOrderType;
  status: SalesStatus;
  paymentMethod: SalesPaymentMethod;
  itemsCount: number;
  subtotal: number;
  deliveryFee: number;
  totalAmount: number;
  createdDate: Date;
}

interface SalesSummary {
  totalSales: number;
  totalOrders: number;
  completedOrders: number;
  pendingOrders: number;
  cancelledOrders: number;
  deliverySales: number;
  averageOrderValue: number;
  activeCashiers: number;
  completionRate: number;
  deliveryShare: number;
  cancellationRate: number;
}

@Component({
  selector: 'app-sales-reports',
  templateUrl: './sales-reports.component.html',
  styleUrls: ['./sales-reports.component.css']
})
export class SalesReportsComponent implements OnInit {
  readonly quickRanges: { label: string; value: SalesQuickRange }[] = [
    { label: 'اليوم', value: 'today' },
    { label: 'آخر 7 أيام', value: 'week' },
    { label: 'هذا الشهر', value: 'month' },
    { label: 'آخر 90 يوم', value: 'quarter' },
    { label: 'كل الفترات', value: 'all' }
  ];

  readonly pageSizeOptions = [6, 8, 10, 12];

  reportRows: SalesReportRow[] = [];
  filteredRows: SalesReportRow[] = [];
  pagedRows: SalesReportRow[] = [];
  summary: SalesSummary = this.createEmptySummary();

  currentPage = 1;
  pageSize = 8;
  selectedRange: SalesQuickRange = 'month';
  selectedStatus: SalesStatus | 'all' = 'all';
  selectedOrderType: SalesOrderType | 'all' = 'all';
  selectedCashier = 'all';
  searchTerm = '';
  dateFrom = '';
  dateTo = '';
  lastUpdated = new Date();

  ngOnInit(): void {
    this.reportRows = this.buildMockSalesReports();
    this.setQuickRange('month');
  }

  get totalCount(): number {
    return this.filteredRows.length;
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  get cashierOptions(): string[] {
    return [...new Set(this.reportRows.map(item => item.cashierName))];
  }

  get activeRangeLabel(): string {
    if (this.selectedRange === 'custom' && (this.dateFrom || this.dateTo)) {
      if (this.dateFrom && this.dateTo) {
        return `${this.dateFrom} - ${this.dateTo}`;
      }

      return this.dateFrom ? `من ${this.dateFrom}` : `حتى ${this.dateTo}`;
    }

    const selectedRange = this.quickRanges.find(item => item.value === this.selectedRange);
    return selectedRange?.label ?? 'نطاق مخصص';
  }

  setQuickRange(range: SalesQuickRange): void {
    this.selectedRange = range;

    if (range === 'all') {
      this.dateFrom = '';
      this.dateTo = '';
      this.applyFilters();
      return;
    }

    const today = new Date();
    const endDate = this.stripTime(today);
    let startDate = this.stripTime(today);

    if (range === 'week') {
      startDate.setDate(endDate.getDate() - 6);
    } else if (range === 'month') {
      startDate = new Date(endDate.getFullYear(), endDate.getMonth(), 1);
    } else if (range === 'quarter') {
      startDate.setDate(endDate.getDate() - 89);
    }

    this.dateFrom = this.toInputDate(startDate);
    this.dateTo = this.toInputDate(endDate);
    this.applyFilters();
  }

  onDateRangeChange(): void {
    this.selectedRange = 'custom';
    this.applyFilters();
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.updatePagedRows();
  }

  refreshReport(): void {
    this.lastUpdated = new Date();
    this.applyFilters(false);
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = 'all';
    this.selectedOrderType = 'all';
    this.selectedCashier = 'all';
    this.pageSize = 8;
    this.currentPage = 1;
    this.setQuickRange('month');
  }

  PageChange(event: any): void {
    this.currentPage = event.page;
    this.updatePagedRows();
  }

  applyFilters(resetPage = true): void {
    const search = this.searchTerm.trim().toLowerCase();
    const fromDate = this.dateFrom ? new Date(`${this.dateFrom}T00:00:00`) : null;
    const toDate = this.dateTo ? new Date(`${this.dateTo}T23:59:59`) : null;

    this.filteredRows = this.reportRows
      .filter(item => {
        const matchesSearch = !search || this.matchesSearch(item, search);
        const matchesStatus = this.selectedStatus === 'all' || item.status === this.selectedStatus;
        const matchesOrderType = this.selectedOrderType === 'all' || item.orderType === this.selectedOrderType;
        const matchesCashier = this.selectedCashier === 'all' || item.cashierName === this.selectedCashier;
        const matchesFromDate = !fromDate || item.createdDate >= fromDate;
        const matchesToDate = !toDate || item.createdDate <= toDate;

        return matchesSearch && matchesStatus && matchesOrderType && matchesCashier && matchesFromDate && matchesToDate;
      })
      .sort((first, second) => second.createdDate.getTime() - first.createdDate.getTime());

    if (resetPage) {
      this.currentPage = 1;
    }

    this.summary = this.calculateSummary(this.filteredRows);
    this.updatePagedRows();
  }

  getStatusLabel(status: SalesStatus): string {
    return {
      completed: 'مكتمل',
      pending: 'قيد التنفيذ',
      cancelled: 'ملغي'
    }[status];
  }

  getOrderTypeLabel(orderType: SalesOrderType): string {
    return {
      takeaway: 'خارجي',
      delivery: 'توصيل'
    }[orderType];
  }

  getPaymentMethodLabel(paymentMethod: SalesPaymentMethod): string {
    return {
      cash: 'نقدي',
      card: 'بطاقة',
      wallet: 'محفظة'
    }[paymentMethod];
  }

  trackByOrder(_index: number, item: SalesReportRow): number {
    return item.orderId;
  }

  private updatePagedRows(): void {
    const safeCurrentPage = Math.min(this.currentPage, this.totalPages);
    const startIndex = (safeCurrentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.currentPage = safeCurrentPage;
    this.pagedRows = this.filteredRows.slice(startIndex, endIndex);
  }

  private matchesSearch(item: SalesReportRow, search: string): boolean {
    return [
      item.orderNumber,
      item.customerName,
      item.cashierName,
      this.getStatusLabel(item.status),
      this.getOrderTypeLabel(item.orderType),
      this.getPaymentMethodLabel(item.paymentMethod)
    ]
      .join(' ')
      .toLowerCase()
      .includes(search);
  }

  private calculateSummary(rows: SalesReportRow[]): SalesSummary {
    const completedRows = rows.filter(item => item.status === 'completed');
    const pendingRows = rows.filter(item => item.status === 'pending');
    const cancelledRows = rows.filter(item => item.status === 'cancelled');
    const totalSales = completedRows.reduce((sum, item) => sum + item.totalAmount, 0);
    const deliverySales = completedRows
      .filter(item => item.orderType === 'delivery')
      .reduce((sum, item) => sum + item.totalAmount, 0);
    const completedOrders = completedRows.length;
    const totalOrders = rows.length;

    return {
      totalSales,
      totalOrders,
      completedOrders,
      pendingOrders: pendingRows.length,
      cancelledOrders: cancelledRows.length,
      deliverySales,
      averageOrderValue: completedOrders ? Math.round(totalSales / completedOrders) : 0,
      activeCashiers: new Set(completedRows.map(item => item.cashierName)).size,
      completionRate: totalOrders ? Math.round((completedOrders / totalOrders) * 100) : 0,
      deliveryShare: totalSales ? Math.round((deliverySales / totalSales) * 100) : 0,
      cancellationRate: totalOrders ? Math.round((cancelledRows.length / totalOrders) * 100) : 0
    };
  }

  private buildMockSalesReports(): SalesReportRow[] {
    return [
      this.createReport(2051, 'أحمد علي', 'محمد ناصر', 'delivery', 'completed', 'cash', 5, 162000, 10000, 0, 10, 15),
      this.createReport(2050, 'سارة محمود', 'محمود سامي', 'takeaway', 'completed', 'card', 3, 89000, 0, 0, 9, 20),
      this.createReport(2049, 'خالد ياسين', 'محمد ناصر', 'delivery', 'pending', 'cash', 4, 118000, 10000, 0, 8, 40),
      this.createReport(2048, 'ريم عبد الله', 'نور الهدى', 'takeaway', 'completed', 'wallet', 2, 56000, 0, 1, 19, 10),
      this.createReport(2047, 'آية حسن', 'نور الهدى', 'delivery', 'completed', 'card', 6, 174000, 12000, 1, 16, 35),
      this.createReport(2046, 'يوسف جمال', 'محمود سامي', 'takeaway', 'cancelled', 'cash', 2, 43000, 0, 2, 14, 50),
      this.createReport(2045, 'نهى أحمد', 'محمد ناصر', 'delivery', 'completed', 'cash', 7, 199000, 15000, 3, 13, 25),
      this.createReport(2044, 'عمر طلال', 'نور الهدى', 'takeaway', 'completed', 'card', 4, 97000, 0, 4, 11, 5),
      this.createReport(2043, 'لينا سامر', 'محمود سامي', 'delivery', 'completed', 'wallet', 3, 121000, 12000, 5, 20, 30),
      this.createReport(2042, 'حسين جابر', 'محمد ناصر', 'takeaway', 'pending', 'cash', 5, 101000, 0, 6, 15, 40),
      this.createReport(2041, 'بتول خالد', 'نور الهدى', 'delivery', 'completed', 'card', 6, 185000, 12000, 7, 18, 15),
      this.createReport(2040, 'رامي إياد', 'محمود سامي', 'takeaway', 'completed', 'cash', 3, 72000, 0, 8, 12, 45),
      this.createReport(2039, 'مريم فارس', 'محمد ناصر', 'delivery', 'cancelled', 'wallet', 4, 112000, 10000, 9, 17, 5),
      this.createReport(2038, 'علي بدر', 'نور الهدى', 'takeaway', 'completed', 'card', 2, 49000, 0, 11, 10, 20),
      this.createReport(2037, 'ديما يوسف', 'محمود سامي', 'delivery', 'completed', 'cash', 8, 214000, 15000, 12, 21, 0),
      this.createReport(2036, 'باسل كرم', 'محمد ناصر', 'takeaway', 'completed', 'wallet', 4, 93000, 0, 15, 9, 45),
      this.createReport(2035, 'هدى صبحي', 'نور الهدى', 'delivery', 'completed', 'cash', 5, 146000, 10000, 18, 13, 30),
      this.createReport(2034, 'سيف حيدر', 'محمود سامي', 'takeaway', 'completed', 'card', 3, 68000, 0, 21, 16, 10)
    ];
  }

  private createReport(
    orderNumber: number,
    customerName: string,
    cashierName: string,
    orderType: SalesOrderType,
    status: SalesStatus,
    paymentMethod: SalesPaymentMethod,
    itemsCount: number,
    subtotal: number,
    deliveryFee: number,
    daysAgo: number,
    hour: number,
    minute: number
  ): SalesReportRow {
    const createdDate = new Date();
    createdDate.setDate(createdDate.getDate() - daysAgo);
    createdDate.setHours(hour, minute, 0, 0);

    return {
      orderId: orderNumber,
      orderNumber,
      customerName,
      cashierName,
      orderType,
      status,
      paymentMethod,
      itemsCount,
      subtotal,
      deliveryFee,
      totalAmount: subtotal + deliveryFee,
      createdDate
    };
  }

  private createEmptySummary(): SalesSummary {
    return {
      totalSales: 0,
      totalOrders: 0,
      completedOrders: 0,
      pendingOrders: 0,
      cancelledOrders: 0,
      deliverySales: 0,
      averageOrderValue: 0,
      activeCashiers: 0,
      completionRate: 0,
      deliveryShare: 0,
      cancellationRate: 0
    };
  }

  private stripTime(date: Date): Date {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
  }

  private toInputDate(date: Date): string {
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');
    return `${date.getFullYear()}-${month}-${day}`;
  }
}
