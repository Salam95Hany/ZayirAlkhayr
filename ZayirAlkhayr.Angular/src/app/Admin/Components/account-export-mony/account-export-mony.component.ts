import { Component, OnInit } from '@angular/core';
import { PagingFilterModel } from '../../Models/PagingFilterModel';
import { AdminService } from '../../Services/admin.service';
import { FilterModel } from '../../Models/FilterModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationFormService } from '../../Services/validation-form.service';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-account-export-mony',
  templateUrl: './account-export-mony.component.html',
  styleUrls: ['./account-export-mony.component.css']
})
export class AccountExportMonyComponent implements OnInit {
  AccountMoneyList: any[] = [];
  ItemForm: FormGroup;
  isFilter = false;
  showLoader = false;
  TotalCount = 0;
  TotalExportValue = 0;
  UserModel: any;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 25
  }

  constructor(private adminService: AdminService, private formService: ValidationFormService
    , private fb: FormBuilder, private modalService: NgbModal, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllAccountsExportMony();
    this.GetAllImportExportMonyStatistics();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      details: ['', [Validators.required, this.formService.noSpaceValidator]],
      totalValue: ['', Validators.required],
      insertUser: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any) {
    this.ResetForm();
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  GetAllAccountsExportMony() {
    this.adminService.GetAllAccountsExportMony(this.PagingFilter).subscribe(data => {
      this.AccountMoneyList = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  GetAllImportExportMonyStatistics() {
    this.adminService.GetAllImportExportMonyStatistics(this.PagingFilter).subscribe(data => {
      this.TotalExportValue = data[0].allExportMoney ?? 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllAccountsExportMony();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllAccountsExportMony();
    this.GetAllImportExportMonyStatistics();
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;
    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    this.showLoader = true;
    this.adminService.AddNewAccountsExportMony(this.ItemForm.value).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllAccountsExportMony();
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
}
