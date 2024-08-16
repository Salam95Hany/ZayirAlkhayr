import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { GeneralStatusService } from 'src/app/Admin/Services/general-status.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-family-needs',
  templateUrl: './family-needs.component.html',
  styleUrls: ['./family-needs.component.css']
})
export class FamilyNeedsComponent implements OnInit {
  FamilyNeedsData: any[] = [];
  FamilyCategoriesData: any[] = [];
  CategoryList: any[] = [];
  NeedFilterList: FilterModel[] = [];
  CategoryFilterList: FilterModel[] = [];
  showLoader = false;
  NeedIsFilter = false;
  CategoryIsFilter = false;
  CategoryValidation = false;
  CategoryName = 'الفئات';
  CategoryId: any;
  NeedId: any;
  NeedTotalCount = 0;
  CategoryTotalCount = 0;
  NeedForm: FormGroup;
  CategoryForm: FormGroup;
  UserModel: any;
  NeedsPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };
  CategoriesPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };

  constructor(private modalService: NgbModal, private generalStatusService: GeneralStatusService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.NeedFormInit();
    this.CategoryFormInit();
    this.GetAllFamilyNeedCategories();
    this.GetAllFamilyNeedTypesData();
    this.GetAllFamilyNeedTypesFilters();
    this.GetAllFamilyNeedCategoriesData();
    this.GetAllFamilyNeedCategoriesFilters();
  }

  NeedFormInit() {
    this.NeedForm = this.fb.group({
      id: 0,
      categoryId: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      InsertUser: null
    });
  }

  CategoryFormInit() {
    this.CategoryForm = this.fb.group({
      id: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      InsertUser: null
    });
  }

  FillEditNeedForm(item: any) {
    this.CategoryName = item.category;
    this.CategoryId = item.categoryId;
    this.NeedForm.setValue({
      id: item.id,
      categoryId: item.categoryId,
      name: item?.name,
      InsertUser: this.UserModel?.userId,
    });
  }

  FillEditCategoryForm(item: any) {
    this.CategoryForm.setValue({
      id: item.id,
      name: item?.name,
      InsertUser: this.UserModel?.userId,
    });
  }

  ResetNeedForm() {
    this.NeedForm.reset();
    this.CategoryName = 'الفئات';
    this.CategoryId = '';
    this.NeedForm.get('categoryId').setValue(0);
    this.NeedForm.get('id').setValue(0);
    this.NeedForm.get('InsertUser').setValue(this.UserModel?.userId);
  }

  ResetCategoryForm() {
    this.CategoryForm.reset();
    this.CategoryForm.get('id').setValue(0);
    this.CategoryForm.get('InsertUser').setValue(this.UserModel?.userId);
  }

  openNeedAddItemModal(content: any, item: any) {
    this.ResetNeedForm();
    this.CategoryValidation = false;
    if (item)
      this.FillEditNeedForm(item);
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openCategoryAddItemModal(content: any, item: any) {
    this.ResetCategoryForm();
    if (item)
      this.FillEditCategoryForm(item);
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  openDeleteNeedItemModal(content: any, item: any) {
    this.NeedId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  openDeleteCategoryItemModal(content: any, item: any) {
    this.CategoryId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllFamilyNeedTypesData() {
    this.generalStatusService.GetAllFamilyNeedTypesData(this.NeedsPagingFilter).subscribe(data => {
      this.FamilyNeedsData = data;
      this.NeedTotalCount = this.FamilyNeedsData && this.FamilyNeedsData.length > 0 ? this.FamilyNeedsData[0].totalCount : 0;
    });
  }

  GetAllFamilyNeedCategories() {
    this.generalStatusService.GetAllFamilyNeedCategories().subscribe(data => {
      this.CategoryList = data;
    });
  }

  GetAllFamilyNeedTypesFilters() {
    this.generalStatusService.GetAllFamilyNeedTypesFilters(this.NeedsPagingFilter).subscribe(data => {
      this.NeedFilterList = data;
    });
  }

  NeedPageChange(obj: any) {
    this.NeedsPagingFilter.currentpage = obj.page;
    this.GetAllFamilyNeedTypesData();
  }

  NeedFilterChecked(filterList: FilterModel[]) {
    debugger;
    this.NeedsPagingFilter.filterList = filterList;
    this.GetAllFamilyNeedTypesData();
  }

  GetAllFamilyNeedCategoriesData() {
    this.generalStatusService.GetAllFamilyNeedCategoriesData(this.CategoriesPagingFilter).subscribe(data => {
      this.FamilyCategoriesData = data;
      this.CategoryTotalCount = this.FamilyCategoriesData && this.FamilyCategoriesData.length > 0 ? this.FamilyCategoriesData[0].totalCount : 0;
    });
  }

  GetAllFamilyNeedCategoriesFilters() {
    this.generalStatusService.GetAllFamilyNeedCategoriesFilters(this.CategoriesPagingFilter).subscribe(data => {
      this.CategoryFilterList = data;
    });
  }

  CategoryPageChange(obj: any) {
    this.CategoriesPagingFilter.currentpage = obj.page;
    this.GetAllFamilyNeedCategoriesData();
  }

  CategoryFilterChecked(filterList: FilterModel[]) {
    debugger
    this.CategoriesPagingFilter.filterList = filterList;
    this.GetAllFamilyNeedCategoriesData();
  }

  OnChangeCategory(item: any) {
    this.CategoryId = item.id;
    this.CategoryName = item.name;
    this.CategoryValidation = false;
  }

  AddNewNeed() {
    this.NeedForm = this.formService.TrimFormInputValue(this.NeedForm);
    let isValid = this.NeedForm.valid;

    this.CategoryValidation = this.CategoryName == 'الفئات';
    if (!isValid || this.CategoryValidation) {
      this.formService.validateAllFormFields(this.NeedForm);
      return;
    }

    this.NeedForm.patchValue({ categoryId: this.CategoryId });

    this.showLoader = true;
    if (this.NeedForm.controls['id'].value == 0) {
      this.generalStatusService.AddNewFamilyNeedType(this.NeedForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllFamilyNeedTypesData();
          this.GetAllFamilyNeedTypesFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.generalStatusService.UpdateFamilyNeedType(this.NeedForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllFamilyNeedTypesData();
          this.GetAllFamilyNeedTypesFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    }
  }

  AddNewCategory() {
    this.CategoryForm = this.formService.TrimFormInputValue(this.CategoryForm);
    let isValid = this.CategoryForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.CategoryForm);
      return;
    }

    this.showLoader = true;
    if (this.CategoryForm.controls['id'].value == 0) {
      this.generalStatusService.AddNewFamilyNeedCategory(this.CategoryForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllFamilyNeedCategoriesData();
          this.GetAllFamilyNeedCategoriesFilters();
          this.GetAllFamilyNeedCategories();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.generalStatusService.UpdateFamilyNeedCategory(this.CategoryForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllFamilyNeedCategoriesData();
          this.GetAllFamilyNeedCategoriesFilters();
          this.GetAllFamilyNeedCategories();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    }
  }

  DeleteNeed() {
    this.showLoader = true;
    this.generalStatusService.DeleteFamilyNeedType(this.NeedId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllFamilyNeedTypesData();
        this.GetAllFamilyNeedTypesFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  DeleteCategory() {
    this.showLoader = true;
    this.generalStatusService.DeleteFamilyNeedCategory(this.CategoryId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllFamilyNeedCategoriesData();
        this.GetAllFamilyNeedCategoriesFilters();
        this.GetAllFamilyNeedCategories();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

}
