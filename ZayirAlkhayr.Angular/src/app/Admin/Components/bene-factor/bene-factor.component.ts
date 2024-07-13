import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../Services/admin-website.service';
import { ValidationFormService } from '../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UploadFileModel } from '../../Models/FileModel';
import { PagingFilterModel } from '../../Models/PagingFilterModel';
import { FilterModel } from '../../Models/FilterModel';

@Component({
  selector: 'app-bene-factor',
  templateUrl: './bene-factor.component.html',
  styleUrls: ['./bene-factor.component.css']
})
export class BeneFactorComponent implements OnInit {
  @ViewChild('InputFile') InputFile: ElementRef;
  isFilter = false;
  BeneFactorData: any[] = [];
  BeneFactorHeaders: any[] = [];
  FileModel: UploadFileModel = {
    files: [],
    deletedFiles: []
  } as UploadFileModel;
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

  constructor(private modalService: NgbModal, private offcanvasService: NgbOffcanvas,
    private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.GetAllBeneFactorData();
    this.GetAllBeneFactorFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      fullName: ['', Validators.required],
      description: ['', Validators.required],
      phone: null,
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
    let fileName = item.image.split('\\');
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
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    this.isFileExist = false;
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

  openCashSidePanel(content: any) {
    this.offcanvasService.open(content, { position: 'end' });
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
    this.ImageFile = null;
    this.formService.onSelectedFile(event.target.files).then(data => {
      this.ImageFile = data[1][0];
      this.isFileExist = false;
    });
  }

  DeleteSelectedFile() {
    this.ImageFile = null;
    this.InputFile.nativeElement.value = '';
  }

  AddNewItem() {
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.ItemForm.patchValue({ file: this.ImageFile });
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
