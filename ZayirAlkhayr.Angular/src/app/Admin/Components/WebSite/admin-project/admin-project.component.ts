import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../../Services/admin-website.service';
import { ValidationFormService } from '../../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FileSortingModel, UploadFileModel } from '../../../Models/FileModel';
import { DatePipe } from '@angular/common';
import { PagingFilterModel } from '../../../Models/PagingFilterModel';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';

@Component({
  selector: 'app-admin-project',
  templateUrl: './admin-project.component.html',
  styleUrls: ['./admin-project.component.css']
})
export class AdminProjectComponent implements OnInit {
  @ViewChild('InputMultiFile') InputMultiFile: ElementRef;
  isFilter = false;
  showLoader = false;
  ProjectsData: any[] = [];
  multiFileURL: any[] = [];
  multiImagesFile: any[] = [];
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
  UserModel: any;
  ProjectId: any;
  TotalCount = 0;

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));;
    this.FormInit();
    this.GetAllProjects();
    this.GetWebsiteAdminFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      title: ['', [Validators.required, this.formService.noSpaceValidator]],
      description: ['', [Validators.required, this.formService.noSpaceValidator]],
      totalDonationAmount: ['', [Validators.required, this.formService.noSpaceValidator,Validators.pattern("[0-9]+")]],
      benefactorCount: ['', [Validators.required, this.formService.noSpaceValidator,Validators.pattern("[0-9]+")]],
      totalAmount: ['', [Validators.required, this.formService.noSpaceValidator,Validators.pattern("[0-9]+")]],
      remainingAmount: ['', [Validators.required, this.formService.noSpaceValidator,Validators.pattern("[0-9]+")]],
      isVisible: true,
      insertUser: null
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      id: item.id,
      title: item.title,
      description: item?.description,
      totalDonationAmount: item?.totalDonationAmount,
      benefactorCount: item?.benefactorCount,
      totalAmount: item?.totalAmount,
      remainingAmount: item?.remainingAmount,
      isVisible: item?.isVisible,
      insertUser: this.UserModel?.userId ?? null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ProjectId = '';
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('isVisible').setValue(true);
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
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

  openAddImagesModal(content: any, item: any) {
    this.multiFileURL = [];
    this.multiImagesFile = [];
    this.FileModel = { files: [], deletedFiles: [] };
    this.ProjectId = item.id;
    this.InputMultiFile.nativeElement.value = '';
    this.adminService.GetProjectsSliderImagesById(item.id).subscribe(data => {
      this.multiFileURL = data;
      this.modalService.open(content, {
        size: 'xl',
        scrollable: true,
        centered: true
      });
    });
  }

  openDeleteItemModal(content: any, item: any) {
    debugger;
    this.ProjectId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllProjects() {
    this.adminService.GetAllProjects(this.PagingFilter).subscribe(data => {
      this.ProjectsData = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllProjects();
  }

  GetWebsiteAdminFilters() {
    this.adminService.GetAllWebPagesFilters('Project').subscribe(data => {
      this.FilterList = data;

    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllProjects();
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

    this.FileModel.id = this.ProjectId;
    this.FileModel.files = this.multiImagesFile.map(i => i.file);
    const formData = new FormData();
    this.formService.buildFormData(formData, this.FileModel);
    this.showLoader = true;
    this.adminService.AddProjectsSliderImage(formData).subscribe(data => {
      if (data.done) {
        this.modalService.dismissAll();
        this.toaster.success(data.message);
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  AddNewEvent() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.showLoader = true;
    if (this.ItemForm.controls['id'].value == 0) {
      this.adminService.AddNewProjects(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllProjects();
          this.GetWebsiteAdminFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.adminService.UpdateProjects(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllProjects();
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
    this.adminService.DeleteProjects(this.ProjectId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllProjects();
        this.GetWebsiteAdminFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
        this.showLoader = false;
    });
  }
}
