import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../../Services/admin-website.service';
import { ValidationFormService } from '../../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PagingFilterModel } from '../../../Models/PagingFilterModel';
import { BeneFactorDetails } from '../../../Models/BeneFactorModel';
import { PDFHeaderSelectedModel, PDFModel } from '../../../Models/PDFHeaderSelected';
import { PdfDownloadService } from '../../../Services/pdf-download.service';
import { DatePipe } from '@angular/common';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';

@Component({
  selector: 'app-bene-factor',
  templateUrl: './bene-factor.component.html',
  styleUrls: ['./bene-factor.component.css']
})
export class BeneFactorComponent implements OnInit {
  @ViewChild('InputFile') InputFile: ElementRef;
  isFilter = false;
  showLoader = false;
  BeneFactorValues: BeneFactorDetails = {} as BeneFactorDetails;
  BeneFactorValuesData: any[] = [];
  BeneFactorData: any[] = [];
  BeneFactorHeaders: any[] = [];
  PDFHeaderModel: PDFHeaderSelectedModel[] = [];
  PDFModel: PDFModel = {
    filterList: [],
    headers: []
  };
  fileURL: any[] = [];
  NationalityList: any[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
  }
  FilterList: FilterModel[] = [];
  ItemForm: FormGroup;
  isFileExist = false;
  ImageFile: any;
  UserModel: any;
  BeneFactorId: any;
  TotalCount = 0;
  RowCount = 25;
  NationalityName = 'الجنسية';
  DefaultImage = '../../../../assets/logo-2.png';
  NationalityValidation = false;
  NationalityId: any;

  constructor(private modalService: NgbModal, private offcanvasService: NgbOffcanvas,
    private adminService: AdminWebsiteService, private formService: ValidationFormService, private datepipe: DatePipe
    , private fb: FormBuilder, private toaster: ToastrService, private pdfService: PdfDownloadService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllBeneFactorNationalities();
    this.GetAllBeneFactorData();
    this.GetAllBeneFactorFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      fullName: ['', [Validators.required, this.formService.noSpaceValidator]],
      description: ['', this.formService.noSpaceValidator],
      phone: ['', [Validators.required, Validators.pattern("[0-9]+")]],
      phone2: ['', Validators.pattern("[0-9]+")],
      address: ['', this.formService.noSpaceValidator],
      nationalityId: null,
      faceBook: ['', this.formService.noSpaceValidator],
      welcomeMessage: ['', this.formService.noSpaceValidator],
      InsertUser: null,
      oldFileName: null,
      file: null,
    });
  }

  FillEditForm(item: any) {
    this.fileURL = [];
    this.fileURL.push(item);
    this.NationalityName = item.nationality;
    this.NationalityId = item.nationalityId;
    let fileName = item.image.split('/');
    this.ItemForm.setValue({
      id: item.id,
      fullName: item.fullName,
      description: item?.description,
      phone: item?.phone,
      phone2: item?.phone2,
      address: item?.address,
      nationalityId: item?.nationalityId,
      faceBook: item?.faceBook,
      welcomeMessage: item?.welcomeMessage,
      oldFileName: fileName[fileName.length - 1],
      InsertUser: this.UserModel?.userId,
      file: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.BeneFactorId = '';
    this.InputFile.nativeElement.value = '';
    this.NationalityName = 'الجنسية';
    this.NationalityId = '';
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
    this.BeneFactorValues.totalValue = null;
    this.BeneFactorValues.paymentDate = null;
    this.GetAllBeneFactorParentById();
    this.offcanvasService.open(content, { position: 'end' });
  }

  BeneFactorValueCollapseClick(item: any) {
    item.isCollapsed = !item.isCollapsed;
    this.BeneFactorValuesData.filter(i => i.id != item.id).forEach(item => {
      item.isCollapsed = false;
    });

    if (item.isCollapsed && (!item.values || item.values.length == 0))
      this.adminService.GetAllBeneFactorCashDetails(this.BeneFactorValues.beneFactorId, item.id).subscribe(data => {
        let obj = this.BeneFactorValuesData.find(i => i.id == item.id);
        if (obj)
          obj.values = data;
      });
  }

  GetAllBeneFactorParentById() {
    this.adminService.GetAllBeneFactorParentById(this.BeneFactorValues.beneFactorId).subscribe(data => {
      this.BeneFactorValuesData = data;
      this.BeneFactorValuesData.forEach(i => i.isCollapsed = false);
    });
  }

  GetAllBeneFactorData() {
    this.adminService.GetAllBeneFactorData(this.PagingFilter).subscribe(data => {
      this.BeneFactorData = data.table;
      this.BeneFactorHeaders = data.table1;
      this.TotalCount = this.BeneFactorData && this.BeneFactorData.length > 0 ? this.BeneFactorData[0].totalCount : 0;
    });
  }

  GetAllBeneFactorNationalities() {
    this.adminService.GetAllBeneFactorNationalities(this.PagingFilter).subscribe(data => {
      this.NationalityList = data;
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
    this.PDFModel.filterList = filterList;
    this.GetAllBeneFactorData();
  }

  OnChangeNationality(item: any) {
    this.NationalityId = item.id;
    this.NationalityName = item.name;
    this.NationalityValidation = false
  }

  onFileChange(event: any) {
    let fileSize = this.formService.getFileSize(event.target.files[0]);
    if (fileSize > 1) {
      this.toaster.warning(`هذا الملف ${event.target.files[0].name} حجمه أكبر من 1 ميجا`);
      return;
    }
    
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
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    this.NationalityValidation = this.NationalityName == 'الجنسية';
    if (!isValid || this.NationalityValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    this.ItemForm.patchValue({ file: this.ImageFile });
    this.ItemForm.patchValue({ nationalityId: this.NationalityId });
    const formData = new FormData();
    this.formService.buildFormData(formData, this.ItemForm.value);
    this.showLoader = true;
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
        this.showLoader = false;
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
        this.showLoader = false;
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

    if (this.BeneFactorValues.totalValue <= 0) {
      this.toaster.warning('برجاء ادخال مبلغ أكبر من صفر');
      return;
    }

    this.BeneFactorValues.beneFactorTypeId = 1;
    this.BeneFactorValues.isParent = true;
    this.BeneFactorValues.isActive = false;
    this.BeneFactorValues.insertUser = this.UserModel?.userId;
    const formData = new FormData();
    this.formService.buildFormData(formData, this.BeneFactorValues);
    this.showLoader = true;
    this.adminService.AddNewBeneFactorDetails(formData).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.BeneFactorValues.totalValue = null;
        this.BeneFactorValues.paymentDate = '';
        this.GetAllBeneFactorParentById();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    })
  }

  DeleteItem() {
    this.showLoader = true;
    this.adminService.DeleteBeneFactor(this.BeneFactorId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllBeneFactorData();
        this.GetAllBeneFactorFilters();
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

  OpenPdfFileItemModal(content: any) {
    this.PDFHeaderModel = this.pdfService.ConverHeaderToPDFModel(this.BeneFactorHeaders);
    this.RowCount = 25;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  DownloadPdfFile() {
    if (this.BeneFactorData.length == 0) {
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
      this.toaster.warning('عدد الاسطر لا يتجاوز 20 سطر');
      return;
    }
    let today = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = 'المتبرعين' + '_' + today;
    this.PDFModel.headers = this.PDFHeaderModel.filter(i => i.isSelected);
    this.showLoader = true;
    this.pdfService.DownloadFile(this.PDFModel, fileName + '.pdf', 'BeneFactor/ExportBeneFactorsPDFFile?RowCount=' + this.RowCount).subscribe(data => {
      this.showLoader = false;
    });
    this.modalService.dismissAll();
  }

  DownloadExcelFile() {
    if (this.BeneFactorData.length == 0) {
      this.toaster.warning('لا يوجد بيانات للتنزيل');
      return;
    }

    let userName = this.UserModel?.userName;
    let today = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = 'المتبرعين' + '_' + today;
    this.PDFModel.headers = this.pdfService.ConverHeaderToPDFModel(this.BeneFactorHeaders);
    this.showLoader = true;
    this.pdfService.DownloadFile(this.PDFModel, fileName + '.xlsx', 'BeneFactor/ExportBeneFactorsExcelFile?UserName=' + userName).subscribe(data => {
      this.showLoader = false;
    });
  }
}
