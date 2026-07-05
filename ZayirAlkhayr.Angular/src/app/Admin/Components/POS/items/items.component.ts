import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent {
  @ViewChild('InputFile') InputFile: ElementRef;
  UserModel: any;
  isFilter = true;
  showLoader = false;
  ItemForm: FormGroup;
  defaultImage = '../../../../assets/PosLogo.jpeg';
  Total = 0;
  CategoryId: any;
  ProductId: any;
  Results: any[] = [];
  Categories: any[] = [];
  fileURL: any[] = [];
  CategoryName = 'اختر فئة';
  CategoryValidation = false;
  ImageFile: any;
  FilterList: FilterModel[] = [
    {
      "categoryName": "SearchText",
      "categoryDisplayName": "بالاسم",
      "itemId": null,
      "itemKey": null,
      "itemValue": null,
      "filterType": "SearchText",
      "isVisible": false
    }
  ];
  CategoryPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 500
  }
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  }


  constructor(private modalService: NgbModal, private adminService: AdminService,
    private formService: ValidationFormService,
    private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.GetAllProducts();
    this.GetAllCategories();
    this.FormInit();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      itemId: 0,
      name: [
        '',
        [
          Validators.required,
          this.formService.noSpaceValidator,
          Validators.pattern(/^[\u0600-\u06FF0-9\s]+$/)
        ]
      ],
      nameEn: [
        '',
        [
          Validators.required,
          this.formService.noSpaceValidator,
          Validators.pattern(/^[A-Za-z0-9\s]+$/)
        ]
      ],
      categoryId: null,
      price: ['', [Validators.required, this.formService.noSpaceValidator]],
      costPrice: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null,
      oldFileName: null,
      file: null,
    });
  }

  FillEditForm(item: any) {
    this.fileURL = [];
    this.fileURL.push(item);
    this.CategoryName = item.categoryName;
    this.CategoryId = item.categoryId;
    let fileName = item.image.split('\\');
    this.ItemForm.setValue({
      itemId: item.productId,
      name: item.productName,
      nameEn: item.productNameEn,
      categoryId: item.categoryId,
      price: item.price,
      costPrice: item?.costPrice ? item?.costPrice : null,
      oldFileName: fileName[fileName.length - 1],
      insertUser: this.UserModel?.userId,
      file: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('itemId').setValue(0);
    this.CategoryId = null;
    this, this.CategoryName = 'اختر فئة';
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
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
    this.ProductId = item.productId
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  onCategoryClicked(item: any) {
    this.CategoryId = item.categoryId;
    this.CategoryName = item.name;
    this.CategoryValidation = false;
  }

  GetAllCategories() {
    this.adminService.GetAllCategories(this.CategoryPagingFilter).subscribe(data => {
      this.Categories = data.results;
    });
  }

  GetAllProducts() {
    this.showLoader = true;
    this.adminService.GetAllProducts(this.PagingFilter).subscribe(data => {
      this.showLoader = false;
      this.Results = data.results;
      this.Total = data.totalCount;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllProducts();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllProducts();
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
    this.CategoryValidation = !this.CategoryId;
    if (!isValid || this.CategoryValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.showLoader = true;
    this.ItemForm.patchValue({ file: this.ImageFile });
    this.ItemForm.patchValue({ categoryId: this.CategoryId });
    const formData = new FormData();
    this.formService.buildFormData(formData, this.ItemForm.value);
    this.showLoader = true;
    if (this.ItemForm.controls['itemId'].value == 0) {
      this.adminService.AddNewProduct(formData).subscribe(data => {
        this.showLoader = false;
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllProducts();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.adminService.UpdateProduct(formData).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllProducts();
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
    this.adminService.DeleteProduct(this.ProductId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllProducts();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
