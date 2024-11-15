import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { Orphans, OrphansDetails, OrphansDetailsModel } from 'src/app/Admin/Models/OrphansModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { OrphansService } from 'src/app/Admin/Services/orphans.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-orphans',
  templateUrl: './orphans.component.html',
  styleUrls: ['./orphans.component.css']
})
export class OrphansComponent implements OnInit {
  OrphansData: any[] = [];
  FamilyStatusData: OrphansDetailsModel[] = [];
  OrphansDetailsData: OrphansDetails[] = [];
  FilterList: FilterModel[] = [];
  OrphanDataObj: any;
  isFilter = false;
  isEditMode = false;
  showLoader = false;
  FamilyStatusValidation = false;
  OrphansDetailsValidation = false;
  TotalCount = 0;
  ItemForm: FormGroup;
  UserModel: any;
  SearchText = '';
  FamilyStatusName = 'اختر حالة';
  OrphansName = 'اختر يتيم';
  FamilyStatusId: any;
  OrphansDetailsId: any;
  OrphanId: any;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };

  constructor(private modalService: NgbModal, private orphansService: OrphansService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllOrphansData();
    this.GetAllOrphansFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      familyStatusId: 0,
      familyDetailsId: 0,
      dateOfBirth: ['', Validators.required],
      rankingBrothers: ['', [Validators.required, this.formService.noSpaceValidator]],
      notes: null,
      isGuaranteed: false,
      InsertUser: null
    });
  }

  FillEditForm(item: any) {
    this.OrphansDetailsId = item.familyDetailsId;
    this.FamilyStatusId = item.familyStatusId;
    this.FamilyStatusName = item.familyName;
    this.OrphansName = item.name;
    this.GetOrphanDetailsByFamilyId();
    this.ItemForm.setValue({
      id: item?.id,
      familyStatusId: item?.familyStatusId,
      familyDetailsId: item?.familyDetailsId,
      dateOfBirth: this.datePipe.transform(item?.dateOfBirth, 'yyyy-MM-dd'),
      rankingBrothers: item?.rankingBrothers,
      notes: item?.notes,
      isGuaranteed: item?.isGuaranteed,
      InsertUser: this.UserModel?.userId
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.SearchText = '';
    this.FamilyStatusId = '';
    this.OrphansDetailsId = '';
    this.FamilyStatusName = 'اختر حالة';
    this.OrphansName = 'اختر يتيم';
    this.OrphansDetailsData = [];
    this.FamilyStatusValidation = false;
    this.OrphansDetailsValidation = false;
    this.GetAllFamilyStatusOrphansType();
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('isGuaranteed').setValue(false);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item) {
      this.isEditMode = true;
      this.FillEditForm(item);
    }

    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteOrphanModal(content: any, item: any) {
    this.OrphanId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  openOpenDetailsModal(content: any, item: any) {
    this.OrphanDataObj = item;
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  GetAllFamilyStatusOrphansType() {
    this.orphansService.GetAllFamilyStatusOrphansType(this.SearchText).subscribe(data => {
      this.FamilyStatusData = data;
    });
  }

  GetOrphanDetailsByFamilyId() {
    this.orphansService.GetOrphanDetailsByFamilyId(this.FamilyStatusId).subscribe(data => {
      this.OrphansDetailsData = data;
    })
  }

  GetAllOrphansData() {
    this.showLoader = true;
    this.orphansService.GetAllOrphansData(this.PagingFilter).subscribe(data => {
      this.OrphansData = data;
      this.TotalCount = this.OrphansData && this.OrphansData.length > 0 ? this.OrphansData[0].totalCount : 0;
      this.showLoader = false;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllOrphansData();
  }

  GetAllOrphansFilters() {
    this.orphansService.GetAllOrphansFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data;
    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllOrphansData();
  }

  FamilyStatusInputSearch() {
    if (this.SearchText.length >= 3)
      this.GetAllFamilyStatusOrphansType();
  }

  OnChangeFamilyStatus(item: OrphansDetailsModel) {
    this.FamilyStatusId = item.familyStatusId;
    this.FamilyStatusName = item.familyStatusName;
    this.OrphansDetailsData = item.orphansDetails;
    this.OrphansDetailsId = '';
    this.OrphansName = 'اختر يتيم';
    this.FamilyStatusValidation = false;
  }

  OnChangeOrphansDetails(item: OrphansDetails) {
    this.OrphansDetailsId = item.familyDetailsId;
    this.OrphansName = item.familyDetailsName;
    this.OrphansDetailsValidation = false;
  }

  AddNewOrphans() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    this.FamilyStatusValidation = this.FamilyStatusName == 'اختر حالة';
    this.OrphansDetailsValidation = this.OrphansName == 'اختر يتيم';
    if (!isValid || this.FamilyStatusValidation || this.OrphansDetailsValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    this.ItemForm.patchValue({ familyStatusId: this.FamilyStatusId });
    this.ItemForm.patchValue({ familyDetailsId: this.OrphansDetailsId });
    this.showLoader = true;
    if (this.ItemForm.controls['id'].value == 0) {
      this.orphansService.AddNewOrphans(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllOrphansData();
          this.GetAllOrphansFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.orphansService.UpdateOrphans(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllOrphansData();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    }
  }

  DeleteOrphans() {
    this.showLoader = true;
    this.orphansService.DeleteOrphans(this.OrphanId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllOrphansData();
        this.GetAllOrphansFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    })
  }
}
