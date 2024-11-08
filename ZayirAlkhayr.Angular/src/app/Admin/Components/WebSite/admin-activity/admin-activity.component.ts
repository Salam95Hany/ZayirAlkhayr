import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../../Services/admin-website.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationFormService } from '../../../Services/validation-form.service';
import { FileSortingModel, UploadFileModel } from '../../../Models/FileModel';
import { ToastrService } from 'ngx-toastr';
import { PagingFilterModel } from '../../../Models/PagingFilterModel';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';

@Component({
  selector: 'app-admin-activity',
  templateUrl: './admin-activity.component.html',
  styleUrls: ['./admin-activity.component.css']
})
export class AdminActivityComponent implements OnInit {
  @ViewChild('InputFile') InputFile: ElementRef;
  @ViewChild('InputMultiFile') InputMultiFile: ElementRef;
  isFilter = false;
  showLoader = false;
  ActivitiesData: any[] = [];
  fileURL: any[] = [];
  multiFileURL: any[] = [];
  multiImagesFile: any[] = [];
  FileSotingModel: FileSortingModel[] = [];
  FileModel: UploadFileModel = {
    files: [],
    deletedFiles: []
  } as UploadFileModel;
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
  ActivityId: any;
  TotalCount = 0;

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));;
    this.FormInit();
    this.GetAllActivities();
    this.GetWebsiteAdminFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      description: ['', [Validators.required, this.formService.noSpaceValidator]],
      isVisible: true,
      InsertUser: null,
      oldFileName: null,
      file: null,
    });
  }

  FillEditForm(item: any) {
    this.fileURL = [];
    this.fileURL.push(item);
    let fileName = item.image.split('/');
    this.ItemForm.setValue({
      id: item.id,
      name: item.name,
      description: item?.description,
      isVisible: item?.isVisible,
      oldFileName: fileName[fileName.length - 1],
      InsertUser: this.UserModel?.userId,
      file: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ActivityId = '';
    this.InputFile.nativeElement.value = '';
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('isVisible').setValue(true);
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

  openAddImagesModal(content: any, item: any) {
    this.multiFileURL = [];
    this.multiImagesFile = [];
    this.FileModel = { files: [], deletedFiles: [] };
    this.ActivityId = item.id;
    this.InputMultiFile.nativeElement.value = '';
    this.adminService.GetActivitySliderImagesById(item.id).subscribe(data => {
      this.multiFileURL = data;
      this.modalService.open(content, {
        size: 'xl',
        scrollable: true,
        centered: true
      });
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.ActivityId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    })
  }

  GetAllActivities() {
    this.adminService.GetAllActivities(this.PagingFilter).subscribe(data => {
      this.ActivitiesData = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllActivities();
  }

  GetWebsiteAdminFilters() {
    this.adminService.GetAllWebPagesFilters('Activity').subscribe(data => {
      this.FilterList = data;

    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllActivities();
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

  onMultiFileChange(event: any) {
    let fileSizeValidate = false;
    [...event.target.files].forEach(element => {
      let fileSize = this.formService.getFileSize(element);
      if (fileSize > 1) {
        this.toaster.warning(`هذا الملف ${element.name} حجمه أكبر من 1 ميجا`);
        fileSizeValidate = true;
      }
    });

    if (fileSizeValidate)
      return;

    this.formService.onSelectedMultiFile([...event.target.files]).then(data => {
      this.multiFileURL.push(...data?.urls);
      this.multiImagesFile.push(...data?.fileContents);
    });
  }

  DeleteSelectedFile() {
    this.ImageFile = null;
    this.fileURL = [];
    this.InputFile.nativeElement.value = '';
  }

  DeleteMultiImageFiles(index: number, item: any) {
    this.multiFileURL.splice(index, 1);
    this.InputMultiFile.nativeElement.value = '';

    if (item?.id) {
      let fileName = item.image.split('\\');
      this.FileModel.deletedFiles.push({ id: item.id, fileName: fileName[fileName.length - 1] });
    } else {
      this.multiImagesFile = this.multiImagesFile.filter(i => i.uniqueId != item.uniqueId);
    }
  }

  AddMultiImagesFile() {
    if (this.multiImagesFile.length == 0 && this.FileModel.deletedFiles.length == 0)
      return;

    this.FileModel.id = this.ActivityId;
    this.FileModel.files = this.multiImagesFile.map(i => i.file);
    const formData = new FormData();
    this.formService.buildFormData(formData, this.FileModel);
    this.showLoader = true;
    this.adminService.AddActivitySliderImage(formData).subscribe(data => {
      if (data.done) {
        this.modalService.dismissAll();
        this.toaster.success(data.message);
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  AddNewActivity() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;
    this.isFileExist = this.fileURL.length == 0;

    if (!isValid || this.isFileExist) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.ItemForm.patchValue({ file: this.ImageFile });
    const formData = new FormData();
    this.formService.buildFormData(formData, this.ItemForm.value);
    this.showLoader = true;
    if (this.ItemForm.controls['id'].value == 0) {
      this.adminService.AddNewActivity(formData).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllActivities();
          this.GetWebsiteAdminFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.adminService.UpdateActivity(formData).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllActivities();
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
    this.adminService.DeleteActivity(this.ActivityId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllActivities();
        this.GetWebsiteAdminFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  ApplyFilesSorting() {
    let checked = this.multiFileURL.every(i => i.id);
    if (!checked) {
      this.toaster.warning('برجاء اضافة الصور الجديدة اولا');
      return;
    }
    
    this.FileSotingModel = this.multiFileURL.map<FileSortingModel>(i => { return { fileId: i.id, displayOrder: i.displayOrder } });
    this.showLoader = true;
    this.adminService.ApplyFilesSorting(this.FileSotingModel, this.ActivityId).subscribe(data => {
      if (data.done) {
        this.modalService.dismissAll();
        this.toaster.success(data.message);
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
