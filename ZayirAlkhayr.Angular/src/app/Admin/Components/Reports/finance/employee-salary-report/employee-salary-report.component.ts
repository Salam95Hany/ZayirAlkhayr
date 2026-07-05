import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { SearchReportModel } from 'src/app/Admin/Models/SearchReportModel';
import { DownloadFileService } from 'src/app/Admin/Services/download-file.service';
import { ReportService } from 'src/app/Admin/Services/report.service';
import { AuthService } from 'src/app/Auth/auth.service';

@Component({
  selector: 'app-employee-salary-report',
  templateUrl: './employee-salary-report.component.html',
  styleUrls: ['./employee-salary-report.component.css']
})
export class EmployeeSalaryReportComponent implements OnInit {
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
  ReportModel: SearchReportModel = {
    reportType: '',
    queryString: [],
    filterList: []
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
    private toaster: ToastrService,
    private authService: AuthService,
    private fileService: DownloadFileService
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
    this.ReportModel.filterList = filters;
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
    this.ReportModel.filterList = this.PagingFilter.filterList;
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

  DownloadExcelFile() {
    if (this.SalesData.length == 0) {
      this.toaster.warning('لا يوجد بيانات للتصدير');
      return;
    }

    this.ReportModel.userName = this.authService.UserNameAr;
    this.ReportModel.reportType = 'DailySalesReport';
    let today = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = 'تقرير المبيعات اليومية' + '_' + today;
    this.isExporting = true;
    this.fileService.DownloadFile(this.ReportModel, fileName + '.xlsx').subscribe(data => {
      this.isExporting = false;
    });
  }
}
