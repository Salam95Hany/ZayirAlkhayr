import { DatePipe } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { ReportExportRequestModel } from 'src/app/Admin/Models/ReportExportRequestModel';
import { ReportService } from 'src/app/Admin/Services/report.service';

@Component({
  selector: 'app-daily-sales-report',
  templateUrl: './daily-sales-report.component.html',
  styleUrls: ['./daily-sales-report.component.css']
})
export class DailySalesReportComponent implements OnInit {
  private readonly reportType = 'DailySalesReport';
  private readonly exportFormat = 'Excel';
  private readonly fallbackFileName = 'daily-sales-report.xlsx';

  StatisticData: any;
  SalesData: any[] = [];
  FilterList: FilterModel[] = [];
  AllFilterList: FilterModel[] = [];
  lastUpdated: Date = new Date();
  TotalCount = 0;
  TotalPages = 0;
  selectedRange = 'all';
  isExporting = false;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };

  quickRanges: { label: string; type: string; startDate: string | null; endDate: string | null }[] = [
    { label: 'كل الفترات', type: 'all', startDate: null, endDate: null },
    { label: 'اليوم', type: 'day', startDate: '', endDate: '' },
    { label: 'آخر 7 أيام', type: 'week', startDate: '', endDate: '' },
    { label: 'هذا الشهر', type: 'month', startDate: '', endDate: '' },
    { label: 'آخر 90 يوم', type: 'quarter', startDate: '', endDate: '' }
  ];

  constructor(
    private reportService: ReportService,
    private datePipe: DatePipe,
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.setQuickRange();
    this.RangeFilterClicked(this.selectedRange);
  }

  get canExport(): boolean {
    return (this.TotalCount || 0) > 0 || this.SalesData.length > 0;
  }

  LoadData(): void {
    this.GetReportDailySalesSummary();
    this.ReportDailySalesDetailsData();
    this.ReportDailySalesDetailsFilter();
    this.lastUpdated = new Date();
  }

  GetReportDailySalesSummary(): void {
    this.reportService.GetReportDailySalesSummary(this.PagingFilter).subscribe(data => {
      this.StatisticData = data.results[0];
    });
  }

  ReportDailySalesDetailsData(): void {
    this.reportService.ReportDailySalesDetailsData(this.PagingFilter).subscribe(data => {
      this.SalesData = data.results;
      this.TotalCount = data.totalCount;
      this.TotalPages = Math.ceil(this.TotalCount / this.PagingFilter.pagesize!);
    });
  }

  ReportDailySalesDetailsFilter(): void {
    this.reportService.ReportDailySalesDetailsFilter(this.PagingFilter).subscribe(data => {
      this.FilterList = (data.results || []).map(filter => {
        if (filter.filterType !== 'DateRange') {
          return filter;
        }

        return {
          ...filter,
          rangeValue: this.getCurrentDateRangeValue()
        };
      });

      this.FilterList = [...this.FilterList];
    });
  }

  FilterChecked(filters: FilterModel[]): void {
    this.PagingFilter.filterList = filters;
    this.selectedRange = this.detectSelectedRange(filters);
    this.PagingFilter.currentpage = 1;
    this.LoadData();
  }

  PageChange(obj: any): void {
    this.PagingFilter.currentpage = obj.page;
    this.LoadData();
  }

  RangeFilterClicked(type: string): void {
    this.selectedRange = type;

    this.FilterList = this.FilterList.map(filter => {
      if (filter.filterType !== 'DateRange') {
        return filter;
      }

      return {
        ...filter,
        rangeValue: this.getRangeValue(type)
      };
    });

    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = this.PagingFilter.filterList.filter(item => item.filterType !== 'DateRange');

    const dateRangeFilter = this.createDateRangeFilter(type);
    if (dateRangeFilter) {
      this.PagingFilter.filterList.push(dateRangeFilter);
    }

    this.LoadData();
  }

  setQuickRange(): void {
    this.quickRanges.forEach(range => {
      if (range.type === 'day') {
        const today = new Date();
        range.startDate = new Date(today.getFullYear(), today.getMonth(), today.getDate()).toISOString();
        range.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
      }

      if (range.type === 'week') {
        const today = new Date();
        range.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
        const startDate = new Date(today);
        startDate.setDate(today.getDate() - 6);
        range.startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate()).toISOString();
      }

      if (range.type === 'month') {
        const today = new Date();
        range.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
        const startDate = new Date(today.getFullYear(), today.getMonth(), 1);
        range.startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate()).toISOString();
      }

      if (range.type === 'quarter') {
        const today = new Date();
        range.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
        const startDate = new Date(today);
        startDate.setDate(today.getDate() - 89);
        range.startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate()).toISOString();
      }
    });
  }

  ExportSalesReport(): void {
    if (this.isExporting) {
      return;
    }

    if (!this.canExport) {
      this.toaster.warning('لا توجد بيانات متاحة للتصدير');
      return;
    }

    this.isExporting = true;

    this.reportService.CreateGeneralReport(this.buildExportRequest()).pipe(finalize(() => this.isExporting = false)).subscribe({
      next: (response) => {
        if (!response.body || response.body.size === 0) {
          this.toaster.warning('لا توجد بيانات متاحة للتصدير');
          return;
        }

        this.downloadReportFile(response);
        this.toaster.success('تم تصدير التقرير بنجاح');
      },
      error: () => {
        this.toaster.error('حدث خطأ أثناء تصدير التقرير');
      }
    });
  }

  trackByOrder(index: number, item: any): string | number {
    return item?.orderId ?? item?.orderNumber ?? index;
  }

  private createDateRangeFilter(type: string): FilterModel | null {
    const quickRange = this.quickRanges.find(range => range.type === type);
    if (!quickRange || type === 'all') {
      return null;
    }

    const existingFilter = this.FilterList.find(item => item.filterType === 'DateRange');
    const from = this.formatRangeDate(quickRange.startDate);
    const to = this.formatRangeDate(quickRange.endDate);

    return {
      categoryName: existingFilter?.categoryName ?? 'DateRange',
      categoryDisplayName: existingFilter?.categoryDisplayName ?? 'تاريخ الطلب',
      filterType: 'DateRange',
      from: from ?? '',
      to: to ?? '',
      rangeValue: {
        startDate: from,
        endDate: to
      }
    };
  }

  private getRangeValue(type: string): { startDate: string | null; endDate: string | null } | null {
    const quickRange = this.quickRanges.find(range => range.type === type);
    const startDate = this.formatRangeDate(quickRange?.startDate ?? null);
    const endDate = this.formatRangeDate(quickRange?.endDate ?? null);

    if (type === 'all' || !startDate || !endDate) {
      return null;
    }

    return {
      startDate,
      endDate
    };
  }

  private getCurrentDateRangeValue(): { startDate: string | null; endDate: string | null } | null {
    const appliedDateRange = this.PagingFilter.filterList.find(item => item.filterType === 'DateRange');

    if (appliedDateRange?.from && appliedDateRange?.to) {
      return {
        startDate: appliedDateRange.from,
        endDate: appliedDateRange.to
      };
    }

    return this.getRangeValue(this.selectedRange);
  }

  private detectSelectedRange(filters: FilterModel[]): string {
    const dateRangeFilter = filters.find(item => item.filterType === 'DateRange');
    if (!dateRangeFilter?.from || !dateRangeFilter?.to) {
      return 'all';
    }

    const matchedRange = this.quickRanges.find(range =>
      this.formatRangeDate(range.startDate) === dateRangeFilter.from &&
      this.formatRangeDate(range.endDate) === dateRangeFilter.to
    );

    return matchedRange?.type ?? 'custom';
  }

  private formatRangeDate(value: string | null): string | null {
    if (!value) {
      return null;
    }

    return this.datePipe.transform(value, 'yyyy-MM-dd');
  }

  private buildExportRequest(): ReportExportRequestModel {
    const userModel = this.getStoredUser();

    return {
      reportType: this.reportType,
      outputFormat: this.exportFormat,
      userName: userModel?.userNameAr || userModel?.userName || 'System',
      culture: 'ar-EG',
      dateFormat: 'yyyy-MM-dd HH:mm',
      fileNamePrefix: 'DailySalesReport',
      queryString: [],
      filterList: this.mapExportFilters(this.PagingFilter.filterList)
    };
  }

  private mapExportFilters(filters: FilterModel[]): FilterModel[] {
    return (filters || []).map(filter => ({
      categoryName: filter.categoryName,
      categoryDisplayName: filter.categoryDisplayName,
      itemId: filter.itemId,
      itemKey: filter.itemKey,
      itemValue: filter.itemValue,
      isChecked: filter.isChecked,
      from: filter.from,
      to: filter.to,
      filterType: filter.filterType,
      isVisible: filter.isVisible,
      displayOrder: filter.displayOrder,
      filterItems: filter.filterItems ? this.mapExportFilters(filter.filterItems) : undefined
    }));
  }

  private downloadReportFile(response: HttpResponse<Blob>): void {
    const fileName = this.resolveFileName(response);
    const fileBlob = new Blob([response.body as BlobPart], {
      type: response.body?.type || 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });
    const objectUrl = window.URL.createObjectURL(fileBlob);
    const link = document.createElement('a');

    link.href = objectUrl;
    link.download = fileName;
    link.click();

    window.setTimeout(() => window.URL.revokeObjectURL(objectUrl), 1000);
  }

  private resolveFileName(response: HttpResponse<Blob>): string {
    const contentDisposition = response.headers.get('content-disposition');
    const fileNameMatch = contentDisposition?.match(/filename\*?=(?:UTF-8''|")?([^\";]+)/i);

    if (fileNameMatch?.[1]) {
      return decodeURIComponent(fileNameMatch[1].replace(/"/g, '').trim());
    }

    return this.fallbackFileName;
  }

  private getStoredUser(): any | null {
    try {
      const userModel = localStorage.getItem('UserModel');
      return userModel ? JSON.parse(userModel) : null;
    } catch {
      return null;
    }
  }
}
