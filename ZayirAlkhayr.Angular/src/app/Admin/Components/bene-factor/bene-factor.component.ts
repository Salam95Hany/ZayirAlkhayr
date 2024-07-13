import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../Services/admin-website.service';
import { ValidationFormService } from '../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PagingFilterModel } from '../../Models/PagingFilterModel';
import { FilterModel } from '../../Models/FilterModel';
import { BeneFactorValues } from '../../Models/BeneFactorModel';

@Component({
  selector: 'app-bene-factor',
  templateUrl: './bene-factor.component.html',
  styleUrls: ['./bene-factor.component.css']
})
export class BeneFactorComponent implements OnInit {
  @ViewChild('InputFile') InputFile: ElementRef;
  isFilter = false;
  BeneFactorValues: BeneFactorValues = {} as BeneFactorValues;
  BeneFactorValuesData: any[] = [];
  BeneFactorData: any[] = [];
  BeneFactorHeaders: any[] = [];
  fileURL: any[] = [];
  NationalityList = ['سورية', 'مصر', 'السودان']
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 5
  }
  FilterList: FilterModel[] = [];
  ItemForm: FormGroup;
  isFileExist = false;
  ImageFile: any;
  UserModel: any;
  BeneFactorId: any;
  TotalCount = 0;
  NationalityName = 'الجنسية';
  NationalityValidation = false;
  NationalityId: any;

  constructor(private modalService: NgbModal, private offcanvasService: NgbOffcanvas,
    private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllBeneFactorData();
    this.GetAllBeneFactorFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      fullName: ['', Validators.required],
      description: null,
      phone: ['', Validators.required],
      phone2: null,
      address: null,
      nationality: null,
      faceBook: null,
      InsertUser: null,
      oldFileName: null,
      file: null,
    });
  }

  FillEditForm(item: any) {
    this.fileURL = [];
    this.fileURL.push(item);
    this.NationalityName = item.nationality;
    let fileName = item.image.split('/');
    this.ItemForm.setValue({
      id: item.id,
      fullName: item.fullName,
      description: item?.description,
      phone: item?.phone,
      phone2: item?.phone2,
      address: item?.address,
      nationality: item?.nationality,
      faceBook: item?.faceBook,
      oldFileName: fileName[fileName.length - 1],
      InsertUser: this.UserModel?.userId,
      file: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.BeneFactorId = '';
    this.InputFile.nativeElement.value = '';
    this.NationalityValidation = false;
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    this.isFileExist = false;
    this.fileURL = [];
    this.ImageFile = null;
    if (item)
      this.FillEditForm(item);

    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.BeneFactorId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  openCashSidePanel(content: any, item: any) {
    this.BeneFactorValues.beneFactorId = item.id;
    this.BeneFactorValues.fullName = item.fullName;
    this.BeneFactorValues.code = item.code;
    this.GetAllBeneFactorValuesById();
    this.offcanvasService.open(content, { position: 'end' });
  }

  GetAllBeneFactorValuesById() {
    this.adminService.GetAllBeneFactorValuesById(this.BeneFactorValues.beneFactorId).subscribe(data => {
      this.BeneFactorValuesData = data;
    });
  }

  GetAllBeneFactorData() {
    this.adminService.GetAllBeneFactorData(this.PagingFilter).subscribe(data => {
      this.BeneFactorData = data.table;
      this.BeneFactorHeaders = data.table1;
      this.TotalCount = this.BeneFactorData && this.BeneFactorData.length > 0 ? this.BeneFactorData[0].totalCount : 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllBeneFactorData();
  }

  GetAllBeneFactorFilters() {
    this.adminService.GetAllBeneFactorFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data;
    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllBeneFactorData();
  }

  onFileChange(event: any) {
    this.fileURL = [];
    this.ImageFile = null;
    this.formService.onSelectedFile(event.target.files).then(data => {
      this.fileURL.push(data[0]);
      this.ImageFile = data[1][0];
      this.isFileExist = false;
    });
  }

  DeleteSelectedFile() {
    this.ImageFile = null;
    this.fileURL = [];
    this.InputFile.nativeElement.value = '';
  }

  AddNewItem() {
    let isValid = this.ItemForm.valid;

    this.NationalityValidation = this.NationalityName == 'الجنسية';
    if (!isValid || this.NationalityValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.ItemForm.patchValue({ file: this.ImageFile });
    this.ItemForm.patchValue({ nationality: this.NationalityName });
    const formData = new FormData();
    this.formService.buildFormData(formData, this.ItemForm.value);
    if (this.ItemForm.controls['id'].value == 0) {
      this.adminService.AddNewBeneFactor(formData).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllBeneFactorData();
          this.GetAllBeneFactorFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    } else {
      this.adminService.UpdateBeneFactor(formData).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllBeneFactorData();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    }
  }

  AddNewBeneFactorValues() {
    if (!this.BeneFactorValues.totalValue) {
      this.toaster.warning('برجاء ادخال مبلغ التبرع');
      return;
    }

    if (!this.BeneFactorValues.paymentDate) {
      this.toaster.warning('برجاء ادخال تاريخ التبرع');
      return;
    }

    this.BeneFactorValues.isActive = false;
    this.BeneFactorValues.insertUser = this.UserModel?.userId;
    this.adminService.AddNewBeneFactorValues(this.BeneFactorValues).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.BeneFactorValues.totalValue = null;
        this.BeneFactorValues.paymentDate = '';
        this.GetAllBeneFactorValuesById();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
    })
  }

  DeleteItem() {
    this.adminService.DeleteBeneFactor(this.BeneFactorId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllBeneFactorData();
        this.GetAllBeneFactorFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
    });
  }

}
