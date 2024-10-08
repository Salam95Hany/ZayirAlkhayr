import { Component, Injector, OnInit } from '@angular/core';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { GeneralStatusService } from 'src/app/Admin/Services/general-status.service';
import { FamilyStatusSidepanelComponent } from '../family-status-sidepanel/family-status-sidepanel.component';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { PDFHeaderSelectedModel, PDFModel } from 'src/app/Admin/Models/PDFHeaderSelected';
import { PdfDownloadService } from 'src/app/Admin/Services/pdf-download.service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-all-family-status',
  templateUrl: './all-family-status.component.html',
  styleUrls: ['./all-family-status.component.css']
})
export class AllFamilyStatusComponent implements OnInit {
  FamilyStatusData: any[] = [];
  FilterList: FilterModel[] = [];
  PDFHeaderModel: PDFHeaderSelectedModel[] = [];
  FamilyStatusHeaders: any[] = [];
  FamilyStatusId: any
  UserModel: any;
  showLoader = false;
  isFilter = false;
  TotalCount = 0;
  RowCount = 25;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  }
  PDFModel: PDFModel = {
    filterList: [],
    headers: []
  };

  constructor(private generalService: GeneralStatusService, private offcanvasService: NgbOffcanvas, private injector: Injector,
    private modalService: NgbModal, private pdfService: PdfDownloadService, private toaster: ToastrService, private datepipe: DatePipe
    , private formService: ValidationFormService
  ) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.GetAllFamilyStatusData();
    this.GetAllFamilyStatusFilter();
  }

  openUpdateFamilyStatusSidePanel(item: any, UpdateMode: boolean, DetailsMode: boolean) {
    const injector = Injector.create({
      providers: [
        { provide: 'FamilyStatusId', useValue: item.id },
        { provide: 'FamilyStatusCode', useValue: item.code },
        { provide: 'FamilyStatusName', useValue: item.statusName },
        { provide: 'UpdateMode', useValue: UpdateMode },
        { provide: 'DetailsMode', useValue: DetailsMode }
      ],
      parent: this.injector
    });

    const ref = this.offcanvasService.open(FamilyStatusSidepanelComponent, {
      injector: injector,
      position: 'end'
    });

    ref.dismissed.subscribe((result: any) => {
      if (result?.reload == 'reload') {
        this.GetAllFamilyStatusData();
        this.GetAllFamilyStatusFilter();
      }
    });
  }

  openDeleteItemModal(content: any, familyStatusId: any) {
    this.FamilyStatusId = familyStatusId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  OpenPdfFileItemModal(content: any) {
    this.PDFHeaderModel = this.pdfService.ConverHeaderToPDFModel(this.FamilyStatusHeaders);
    this.RowCount = 25;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }



  GetAllFamilyStatusData() {
    this.showLoader = true;
    this.generalService.GetAllFamilyStatusData(this.PagingFilter).subscribe(data => {
      this.showLoader = false;
      this.FamilyStatusData = data.table;
      this.FamilyStatusHeaders = data.table1;
      this.TotalCount = this.FamilyStatusData && this.FamilyStatusData.length > 0 ? this.FamilyStatusData[0].totalCount : 0;
    });
  }

  GetAllFamilyStatusFilter() {
    this.generalService.GetAllFamilyStatusFilter(this.PagingFilter).subscribe(data => {
      this.FilterList = data;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllFamilyStatusData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.PDFModel.filterList = filterList;
    this.GetAllFamilyStatusData();
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }

  DeleteItem() {
    this.showLoader = true;
    this.generalService.DeleteFamilyStatus(this.FamilyStatusId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllFamilyStatusData();
        this.GetAllFamilyStatusFilter();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    })
  }

  DownloadPdfFile() {
    if (this.FamilyStatusData.length == 0) {
      this.toaster.warning('لا يوجد بيانات للتنزيل');
      return;
    }

    let checked = this.PDFHeaderModel.filter(i => i.isSelected);
    let isAllowSummation = this.PDFHeaderModel.filter(i => i.isAllowSummation);
    if (isAllowSummation.length > 1) {
      this.toaster.warning('لا يمكن اختيار جمع قيم العامود الا لعامود واحد فقط');
      return;
    }

    if (checked.length == 0) {
      this.toaster.warning('اختر عامود واحد على الاقل');
      return;
    }

    if (checked.length > 6) {
      this.toaster.warning('لا يمكن اختيار أكثر من 6 أعمدة');
      return;
    }

    if (this.RowCount == 0 || !this.RowCount) {
      this.toaster.warning('أدخل عدد الاسطر');
      return;
    }

    if (this.RowCount > 25) {
      this.toaster.warning('عدد الاسطر لا يتجاوز 25 سطر');
      return;
    }
    let today = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = 'الحالات' + '_' + today;
    this.PDFModel.headers = this.PDFHeaderModel.filter(i => i.isSelected);
    this.showLoader = true;
    this.pdfService.DownloadFile(this.PDFModel, fileName + '.pdf', 'FamilyStatus/ExportFamilyStatusDataPDFFile?RowCount=' + this.RowCount).subscribe(data => {
      this.showLoader = false;
    });
    this.modalService.dismissAll();
  }

  DownloadExcelFile() {
    if (this.FamilyStatusData.length == 0) {
      this.toaster.warning('لا يوجد بيانات للتنزيل');
      return;
    }

    let userName = this.UserModel?.userName;
    let today = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = 'المتبرعين' + '_' + today;
    this.PDFModel.headers = this.pdfService.ConverHeaderToPDFModel(this.FamilyStatusHeaders);
    this.showLoader = true;
    this.pdfService.DownloadFile(this.PDFModel, fileName + '.xlsx', 'FamilyStatus/ExportFamilyStatusDataExcelFile?UserName=' + userName).subscribe(data => {
      this.showLoader = false;
    });
  }
}
