import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../../Services/admin-website.service';
import { ValidationFormService } from '../../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PagingFilterModel } from '../../../Models/PagingFilterModel';

@Component({
  selector: 'app-bene-factor-details',
  templateUrl: './bene-factor-details.component.html',
  styleUrls: ['./bene-factor-details.component.css']
})
export class BeneFactorDetailsComponent implements OnInit {
  @ViewChild('InputFile') InputFile: ElementRef;
  showLoader = false;
  BenefactorType = 'All';
  BeneFactorData: any[] = [];
  fileURL: any[] = [];
  BeneFactorValuesData: any[] = [];
  BeneFactorDetailsData: any[] = [];
  BeneFactorTypesData: any[] = [];
  BeneFactorId: any;
  BeneFactorName = 'متبرع';
  BeneFactorValueId: any;
  BeneFactorValueName = 'قيم التبرع';
  BeneFactorTypeName = 'نوع التبرع';
  DefaultImage = '../../../../assets/logo-2.png';
  BeneFactorTypeId: any;
  UserModel: any;
  Code: any;
  SearchText = '';
  BeneFactorTypeSearchText = '';
  TotalValue = 0;
  TotalCount = 0;
  BeneFactorTotalValue = 0;
  isFileExist = false;
  BeneFactorTypeValidation = false;
  TypeSwitcher = false;
  ImageFile: any;
  ItemForm: FormGroup;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
  }
  BeneFactorPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 200
  }

  constructor(private modalService: NgbModal,
    private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllBeneFactorData();
    this.GetAllBeneFactorTypes();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      beneFactorId: null,
      beneFactorTypeId: null,
      parentId: null,
      details: ['', this.formService.noSpaceValidator],
      totalValue: ['', Validators.required],
      paymentDate: ['', Validators.required],
      insertUser: null,
      isFinalSubscribe: false,
      isParent: false,
      oldFileName: null,
      file: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.isFileExist = false;
    this.fileURL = [];
    this.ImageFile = null;
    this.BeneFactorTypeValidation = false;
    this.InputFile.nativeElement.value = '';
    this.BeneFactorTypeId = this.BenefactorType == 'Cash' ? 1 : null;;
    this.BeneFactorTypeName = this.BenefactorType == 'Cash' ? 'كاش' : 'نوع التبرع';
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('totalValue').setValue(0);
    this.ItemForm.get('isFinalSubscribe').setValue(false);
    this.ItemForm.get('isParent').setValue(false);
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    if (!this.BeneFactorId) {
      this.toaster.warning('برجاء اختيار متبرع');
      return;
    }

    if (this.BenefactorType == 'Cash') {
      if (!this.BeneFactorValueId) {
        this.toaster.warning('برجاء اختيار قيمة التبرع');
        return;
      }
    }
    this.ResetForm();
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  GetBenefactorType(isSelected: boolean) {
    this.BenefactorType = isSelected ? 'Cash' : 'All';
    if (this.BenefactorType == 'Cash') {
      this.BeneFactorTypeId = 1;
      this.BeneFactorTypeName = 'كاش';
    } else {
      this.BeneFactorTypeId = null;
      this.BeneFactorTypeName = 'نوع التبرع';
      this.BeneFactorValueId = null;
      this.BeneFactorValueName = 'قيم التبرع';
      this.TotalValue = 0;
      this.PagingFilter.currentpage = 1;
      this.GetAllBeneFactorDetails();
    }
  }

  OnChangeBeneFactor(item: any) {
    this.BeneFactorId = item.id;
    this.BeneFactorName = item.fullName;
    this.Code = item.code;
    this.BeneFactorValueId = null;
    this.BeneFactorValueName = 'قيم التبرع';
    this.BeneFactorTypeId = null;
    this.BeneFactorTypeName = 'نوع التبرع';
    this.BenefactorType = 'All';
    this.TypeSwitcher = false;
    this.TotalValue = 0;
    this.GetAllBeneFactorParentById();
    this.GetAllBeneFactorDetails();
  }

  OnChangeBeneFactorValue(item: any) {
    this.TotalValue = 0;
    this.BeneFactorValueId = item.id;
    this.TotalValue = item.totalValue;
    this.BeneFactorTotalValue = item.totalValue;
    this.BeneFactorValueName = item.totalValue + '-' + item.paymentDateStr;
    this.GetAllBeneFactorDetailsByValueId();
  }

  OnChangeBeneFactorType(item: any) {
    this.BeneFactorTypeId = item.id;
    this.BeneFactorTypeName = item.name;
    this.BeneFactorTypeValidation = false;
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

  GetAllBeneFactorData() {
    this.adminService.GetAllBeneFactorData(this.BeneFactorPagingFilter).subscribe(data => {
      this.BeneFactorData = data.table;
    });
  }

  GetAllBeneFactorParentById() {
    this.adminService.GetAllBeneFactorParentById(this.BeneFactorId).subscribe(data => {
      this.BeneFactorValuesData = data.filter(i => !i.isActive);
    });
  }

  GetAllBeneFactorDetails() {
    this.adminService.GetAllBeneFactorDetails(this.PagingFilter, this.BeneFactorId).subscribe(data => {
      this.BeneFactorDetailsData = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllBeneFactorDetails();
  }

  GetAllBeneFactorDetailsByValueId() {
    this.adminService.GetAllBeneFactorCashDetails(this.BeneFactorId, this.BeneFactorValueId).subscribe(data => {
      this.BeneFactorDetailsData = data;
      let detailsTotalValue = 0;
      this.BeneFactorDetailsData.filter(i => i.beneFactorTypeId == 1).forEach(i => {
        detailsTotalValue += Number(i.totalValue);
      })
      this.TotalValue = this.BeneFactorTotalValue - detailsTotalValue;
    });
  }

  GetAllBeneFactorTypes() {
    this.adminService.GetAllBeneFactorTypes(this.BeneFactorPagingFilter).subscribe(data => {
      this.BeneFactorTypesData = data.filter(i => i.id != 1);
    });
  }

  AddNewItem() {
    let num = Number(this.ItemForm.controls['totalValue'].value);
    let isFinalSubscribe = this.TotalValue - num;
    if (this.BenefactorType == 'Cash') {
      this.ItemForm.get('parentId').setValue(this.BeneFactorValueId);
      if (this.TotalValue == 0) {
        this.toaster.warning('لا يمكن اضافة تبرع جديد لقد نفذ مبلغ التبرع');
        return;
      }

      if (num > this.TotalValue) {
        this.toaster.warning('لا يمكن اضافة قيمة اكبر من باق مبلغ التبرع');
        return;
      }
    }
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;
    this.BeneFactorTypeValidation = this.BeneFactorTypeName.startsWith('نوع التبرع');
    if (!isValid || this.BeneFactorTypeValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    if (isFinalSubscribe == 0)
      this.ItemForm.get('isFinalSubscribe').setValue(true);

    this.ItemForm.get('beneFactorId').setValue(this.BeneFactorId);

    this.ItemForm.get('beneFactorTypeId').setValue(this.BeneFactorTypeId);
    this.ItemForm.get('file').setValue(this.ImageFile);
    const formData = new FormData();
    this.formService.buildFormData(formData, this.ItemForm.value);
    this.showLoader = true;
    this.adminService.AddNewBeneFactorDetails(formData).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        if (this.BenefactorType == 'Cash')
          this.GetAllBeneFactorDetailsByValueId();
        else
          this.GetAllBeneFactorDetails();

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
