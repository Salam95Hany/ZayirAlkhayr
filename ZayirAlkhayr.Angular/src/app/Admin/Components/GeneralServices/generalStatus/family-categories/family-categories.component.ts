import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { GeneralStatusService } from 'src/app/Admin/Services/general-status.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-family-categories',
  templateUrl: './family-categories.component.html',
  styleUrls: ['./family-categories.component.css']
})
export class FamilyCategoriesComponent implements OnInit {
  FamilyNationalityData: any[] = [];
  FilterList: FilterModel[] = [];
  showLoader = false;
  isFilter = false;
  TotalCount = 0;
  ItemForm: FormGroup;
  UserModel: any;
  NationalityId: any;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };

  constructor(private modalService: NgbModal, private generalStatusService: GeneralStatusService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllFamilyCategoryData();
    this.GetAllFamilyCategoryFilter();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      InsertUser: null
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      id: item.id,
      name: item?.name,
      InsertUser: this.UserModel?.userId,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item)
      this.FillEditForm(item);
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.NationalityId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllFamilyCategoryData() {
    this.generalStatusService.GetAllFamilyCategoryData(this.PagingFilter).subscribe(data => {
      this.FamilyNationalityData = data;
      this.TotalCount = this.FamilyNationalityData && this.FamilyNationalityData.length > 0 ? this.FamilyNationalityData[0].totalCount : 0;
    });
  }

  GetAllFamilyCategoryFilter() {
    this.generalStatusService.GetAllFamilyCategoryFilter(this.PagingFilter).subscribe(data => {
      this.FilterList = data;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllFamilyCategoryData();
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }


    this.showLoader = true;
    if (this.ItemForm.controls['id'].value == 0) {
      this.generalStatusService.AddNewFamilyCategory(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllFamilyCategoryData();
          this.GetAllFamilyCategoryFilter();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.generalStatusService.UpdateFamilyCategory(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllFamilyCategoryData();
          this.GetAllFamilyCategoryFilter();
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
    this.generalStatusService.DeleteFamilyCategory(this.NationalityId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllFamilyCategoryData();
        this.GetAllFamilyCategoryFilter();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
