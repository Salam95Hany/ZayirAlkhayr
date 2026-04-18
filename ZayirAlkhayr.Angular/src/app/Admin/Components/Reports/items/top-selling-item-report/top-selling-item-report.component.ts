import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { ReportService } from 'src/app/Admin/Services/report.service';

@Component({
  selector: 'app-top-selling-item-report',
  templateUrl: './top-selling-item-report.component.html',
  styleUrls: ['./top-selling-item-report.component.css']
})
export class TopSellingItemReportComponent implements OnInit {
StatisticData: any;
  SalesData: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryDisplayName: 'تاريخ الطلب',
      categoryName: 'DateRange',
      filterType: 'DateRange',
      isVisible: true
    }
  ];
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
  }
  quickRanges: { label: string, type: string, startDate: string, endDate: string }[] = [
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

  LoadData() {
    this.GetReportTopSellingItemSummary();
    this.GetReportTopSellingItemsData();
  }

  GetReportTopSellingItemSummary() {
    this.reportService.GetReportTopSellingItemSummary(this.PagingFilter).subscribe(data => {
      this.StatisticData = data.results[0];
    });
  }

  GetReportTopSellingItemsData() {
    this.reportService.GetReportTopSellingItemsData(this.PagingFilter).subscribe(data => {
      this.SalesData = data.results;
      this.TotalCount = data.totalCount;
      this.TotalPages = Math.ceil(this.TotalCount / this.PagingFilter.pagesize!);
    });
  }

  FilterChecked(filters: FilterModel[]) {
    this.PagingFilter.filterList = filters;
    this.selectedRange = this.detectSelectedRange(filters);
    this.PagingFilter.currentpage = 1;
    this.LoadData();
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.LoadData();
  }

  RangeFilterClicked(type: string) {
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

    this.PagingFilter.filterList = this.PagingFilter.filterList.filter(i => i.filterType !== 'DateRange');

    const dateRangeFilter = this.createDateRangeFilter(type);
    if (dateRangeFilter) {
      this.PagingFilter.filterList.push(dateRangeFilter);
    }

    this.LoadData();
  }

  setQuickRange() {
    this.quickRanges.forEach(i => {
      if (i.type === 'day') {
        const today = new Date();
        i.startDate = new Date(today.getFullYear(), today.getMonth(), today.getDate()).toISOString();
        i.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
      }
      if (i.type === 'week') {
        const today = new Date();
        i.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
        const startDate = new Date(today);
        startDate.setDate(today.getDate() - 6);
        i.startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate()).toISOString();
      }
      if (i.type === 'month') {
        const today = new Date();
        i.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
        const startDate = new Date(today.getFullYear(), today.getMonth(), 1);
        i.startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate()).toISOString();
      }
      if (i.type === 'quarter') {
        const today = new Date();
        i.endDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59).toISOString();
        const startDate = new Date(today);
        startDate.setDate(today.getDate() - 89);
        i.startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate()).toISOString();
      }
    });
  }

  private createDateRangeFilter(type: string): FilterModel | null {
    const quickRange = this.quickRanges.find(r => r.type === type);
    if (!quickRange || type === 'all') {
      return null;
    }

    const existingFilter = this.FilterList.find(i => i.filterType === 'DateRange');
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

  private getRangeValue(type: string): { startDate: any; endDate: any } | null {
    const quickRange = this.quickRanges.find(r => r.type === type);
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

  private getCurrentDateRangeValue(): { startDate: any; endDate: any } | null {
    const appliedDateRange = this.PagingFilter.filterList.find(i => i.filterType === 'DateRange');

    if (appliedDateRange?.from && appliedDateRange?.to) {
      return {
        startDate: appliedDateRange.from,
        endDate: appliedDateRange.to
      };
    }

    return this.getRangeValue(this.selectedRange);
  }

  private detectSelectedRange(filters: FilterModel[]): string {
    const dateRangeFilter = filters.find(i => i.filterType === 'DateRange');
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

  ExportSalesReport() {
    if (this.isExporting) {
      return;
    }

    if (!this.canExport) {
      this.toaster.warning('لا توجد بيانات متاحة للتصدير');
      return;
    }

    this.isExporting = true;

    const exportFilter: PagingFilterModel = {
      ...this.PagingFilter,
      currentpage: 1,
      pagesize: Math.max(this.TotalCount || this.SalesData.length, this.PagingFilter.pagesize || 20)
    };

    this.reportService.ReportDailySalesDetailsData(exportFilter)
      .pipe(finalize(() => this.isExporting = false))
      .subscribe({
        next: (data) => {
          const rows = data?.results || [];

          if (!rows.length) {
            this.toaster.warning('لا توجد بيانات متاحة للتصدير');
            return;
          }

          this.toaster.success('تم تصدير التقرير بنجاح');
        },
        error: () => {
          this.toaster.error('حدث خطأ أثناء تصدير التقرير');
        }
      });
  }
}
