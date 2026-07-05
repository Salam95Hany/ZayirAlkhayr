import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent {
  @ViewChild('InputFile') InputFile: ElementRef;
  UserModel: any;
  isFilter = true;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  CategoryId: any;
  defaultImage = '../../../../assets/Dams_Star.png';
  ImageFile: any;
  Results: any[] = [];
  fileURL: any[] = [];
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
    this.GetAllCategories();
    this.FormInit();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      categoryId: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null,
      oldFileName: null,
      file: null,
    });
  }

  FillEditForm(item: any) {
    this.fileURL = [];
    this.fileURL.push(item);
    let fileName = item.image.split('\\');
    this.ItemForm.setValue({
      categoryId: item.categoryId,
      name: item.name,
      oldFileName: fileName[fileName.length - 1],
      insertUser: this.UserModel?.userId,
      file: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('categoryId').setValue(0);
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    this.fileURL = [];
    this.ImageFile = null;
    if (item)
      this.FillEditForm(item);

    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.CategoryId = item.categoryId
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllCategories() {
    this.showLoader = true;
    this.adminService.GetAllCategories(this.PagingFilter).subscribe(data => {
      this.showLoader = false;
      this.Results = data.results;
      this.Total = data.totalCount;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllCategories();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllCategories();
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

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.showLoader = true;
    this.ItemForm.patchValue({ file: this.ImageFile });
    const formData = new FormData();
    this.formService.buildFormData(formData, this.ItemForm.value);
    this.showLoader = true;
    if (this.ItemForm.controls['categoryId'].value == 0) {
      this.adminService.AddNewCategory(formData).subscribe(data => {
        this.showLoader = false;
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllCategories();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.adminService.UpdateCategory(formData).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllCategories();
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
    this.adminService.DeleteCategory(this.CategoryId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllCategories();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
