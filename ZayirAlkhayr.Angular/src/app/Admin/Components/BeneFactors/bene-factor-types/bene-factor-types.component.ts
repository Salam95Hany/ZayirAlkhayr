import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../../Services/admin-website.service';
import { ValidationFormService } from '../../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PagingFilterModel } from '../../../Models/PagingFilterModel';
import { FilterModel } from '../../../Models/FilterModel';

@Component({
  selector: 'app-bene-factor-types',
  templateUrl: './bene-factor-types.component.html',
  styleUrls: ['./bene-factor-types.component.css']
})
export class BeneFactorTypesComponent implements OnInit {
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };
  isFilter = false;
  showLoader = false;
  TotalCount = 0;
  BeneFactorTypesData: any[] = [];
  ItemForm: FormGroup;
  UserModel: any;

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllBeneFactorTypes();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      InsertUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any) {
    this.ResetForm();
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  GetAllBeneFactorTypes() {
    this.adminService.GetAllBeneFactorTypes(this.PagingFilter).subscribe(data => {
      this.BeneFactorTypesData = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  pageChanged(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllBeneFactorTypes();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllBeneFactorTypes();
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.showLoader = true;
    this.adminService.AddNewBeneFactorType(this.ItemForm.value).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllBeneFactorTypes();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

}
