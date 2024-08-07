import { Component, OnInit } from '@angular/core';
import { PagingFilterModel } from '../../../Models/PagingFilterModel';
import { AdminService } from '../../../Services/admin.service';
import { FilterModel } from '../../../Models/FilterModel';
import { ValidationFormService } from '../../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AdminWebsiteService } from '../../../Services/admin-website.service';
import { PdfDownloadService } from '../../../Services/pdf-download.service';
import { DatePipe } from '@angular/common';
import { PDFModel } from '../../../Models/PDFHeaderSelected';

@Component({
  selector: 'app-account-import-mony',
  templateUrl: './account-import-mony.component.html',
  styleUrls: ['./account-import-mony.component.css']
})
export class AccountImportMonyComponent implements OnInit {
  AccountMoneyList: any[] = [];
  BeneFactors: any[] = [];
  BeneFactorTypes: any[] = [];
  ItemForm: FormGroup;
  isFilter = false;
  showLoader = false;
  BeneFactorTypeValidation = false;
  BeneFactorValidation = false;
  TotalCount = 0;
  TotalImportValue = 0;
  UserModel: any;
  BeneFactorName = 'متبرع';
  BeneFactorTypeName = 'نوع التبرع';
  BeneFactorSearchText = '';
  BeneFactorTypeSearchText = '';
  AccountId: any;
  BeneFactorId: any;
  BeneFactorTypeId: any;
  FilterList: FilterModel[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  }
  PDFModel: PDFModel = {
    filterList: [],
    headers: []
  };

  BeneFactorPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 200
  }

  constructor(private adminService: AdminService, private formService: ValidationFormService, private WebService: AdminWebsiteService
    , private fb: FormBuilder, private modalService: NgbModal, private toaster: ToastrService, private pdfService: PdfDownloadService
    , private datepipe: DatePipe) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllAccountsImportMonyData();
    this.GetAllAccountsImportMonyFilters();
    this.GetAllImportExportMonyStatistics();
    this.GetAllBeneFactorData();
    this.GetAllBeneFactorTypes();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      beneFactorId: 0,
      beneFactorTypeId: 0,
      details: ['', [Validators.required, this.formService.noSpaceValidator]],
      totalValue: ['', Validators.required],
      insertUser: null,
      InsertDate: ['', Validators.required],
    });
  }

  FillEditForm(item: any) {
    debugger;
    this.BeneFactorName = item?.fullName;
    this.BeneFactorTypeName = item?.name;
    this.BeneFactorId = item?.beneFactorId;
    this.BeneFactorTypeId = item?.beneFactorTypeId;
    this.ItemForm.setValue({
      id: item.id,
      beneFactorId: item?.beneFactorId,
      beneFactorTypeId: item?.beneFactorTypeId,
      details: item?.details,
      totalValue: item?.totalValue,
      insertUser: this.UserModel?.userId,
      InsertDate: this.datepipe.transform(item?.insertDate, 'yyyy-MM-dd')
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.BeneFactorTypeValidation = false;
    this.BeneFactorValidation = false;
    this.BeneFactorId = '';
    this.BeneFactorName = 'متبرع';
    this.BeneFactorTypeId = '';
    this.BeneFactorTypeName = 'نوع التبرع';
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('beneFactorId').setValue(0);
    this.ItemForm.get('beneFactorTypeId').setValue(0);
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item)
      this.FillEditForm(item);
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.AccountId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    })
  }

  GetAllAccountsImportMonyData() {
    this.adminService.GetAllAccountsImportMonyData(this.PagingFilter).subscribe(data => {
      this.AccountMoneyList = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  GetAllAccountsImportMonyFilters() {
    this.adminService.GetAllAccountsImportMonyFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data;
    });
  }

  GetAllBeneFactorData() {
    this.WebService.GetAllBeneFactorData(this.BeneFactorPagingFilter).subscribe(data => {
      this.BeneFactors = data.table;
    });
  }

  GetAllBeneFactorTypes() {
    this.WebService.GetAllBeneFactorTypes(this.BeneFactorPagingFilter).subscribe(data => {
      this.BeneFactorTypes = data;
    });
  }

  GetAllImportExportMonyStatistics() {
    this.adminService.GetAllImportExportMonyStatistics(this.PagingFilter).subscribe(data => {
      this.TotalImportValue = data[0].importMoney ?? 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllAccountsImportMonyData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllAccountsImportMonyData();
    this.GetAllImportExportMonyStatistics();
  }

  OnChangeBeneFactor(item: any) {
    this.BeneFactorId = item.id;
    this.BeneFactorName = item.fullName;
    this.BeneFactorValidation = false;
  }

  OnChangeBeneFactorType(item: any) {
    this.BeneFactorTypeId = item.id;
    this.BeneFactorTypeName = item.name;
    this.BeneFactorTypeValidation = false;
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;
    this.BeneFactorValidation = this.BeneFactorName.startsWith('متبرع');
    this.BeneFactorTypeValidation = this.BeneFactorTypeName.startsWith('نوع التبرع');
    if (!isValid || this.BeneFactorTypeValidation || this.BeneFactorValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    this.ItemForm.get('beneFactorId').setValue(this.BeneFactorId);
    this.ItemForm.get('beneFactorTypeId').setValue(this.BeneFactorTypeId);
    this.showLoader = true;
    if (this.ItemForm.controls['id'].value == 0) {
      this.adminService.AddNewAccountsImportMony(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllAccountsImportMonyData();
          this.GetAllAccountsImportMonyFilters();
          this.GetAllImportExportMonyStatistics();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.showLoader = true;
      this.adminService.UpdateAccountsImportMony(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllAccountsImportMonyData();
          this.GetAllAccountsImportMonyFilters();
          this.GetAllImportExportMonyStatistics();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    }
  }

  DeleteItem() {
    this.showLoader = true;
    this.adminService.DeleteAccountsImportMony(this.AccountId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllAccountsImportMonyData();
        this.GetAllAccountsImportMonyFilters();
        this.GetAllImportExportMonyStatistics();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }

  DownloadExcelFile() {
    if (this.AccountMoneyList.length == 0) {
      this.toaster.warning('لا يوجد بيانات للتنزيل');
      return;
    }

    let userName = this.UserModel?.userName;
    let today = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = 'الايرادات' + '_' + today;
    this.PDFModel.filterList = this.PagingFilter.filterList;
    this.showLoader = true;
    this.pdfService.DownloadFile(this.PDFModel, fileName + '.xlsx', 'AccountsMony/ExportAccountsImportMonyExcelFile?UserName=' + userName).subscribe(data => {
      this.showLoader = false;
    });
  }

}
