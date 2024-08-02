import { Component, OnInit } from '@angular/core';
import { PagingFilterModel } from '../../Models/PagingFilterModel';
import { AdminService } from '../../Services/admin.service';
import { FilterModel } from '../../Models/FilterModel';
import { ValidationFormService } from '../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-account-import-mony',
  templateUrl: './account-import-mony.component.html',
  styleUrls: ['./account-import-mony.component.css']
})
export class AccountImportMonyComponent implements OnInit {
  AccountMoneyList: any[] = [];
  ItemForm: FormGroup;
  isFilter = false;
  showLoader = false;
  TotalCount = 0;
  TotalImportValue = 0;
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
    this.GetAllAccountsImportMony();
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

  GetAllAccountsImportMony() {
    this.adminService.GetAllAccountsImportMony(this.PagingFilter).subscribe(data => {
      this.AccountMoneyList = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  GetAllImportExportMonyStatistics() {
    this.adminService.GetAllImportExportMonyStatistics(this.PagingFilter).subscribe(data => {
      this.TotalImportValue = data[0].importMoney ?? 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllAccountsImportMony();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllAccountsImportMony();
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
    this.adminService.AddNewAccountsImportMony(this.ItemForm.value).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllAccountsImportMony();
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
