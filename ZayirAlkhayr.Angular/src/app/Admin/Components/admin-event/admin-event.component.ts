import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../Services/admin-website.service';
import { ValidationFormService } from '../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UploadFileModel } from '../../Models/FileModel';
import { DatePipe } from '@angular/common';
import { PagingFilterModel } from '../../Models/PagingFilterModel';
import { FilterModel } from '../../Models/FilterModel';

@Component({
  selector: 'app-admin-event',
  templateUrl: './admin-event.component.html',
  styleUrls: ['./admin-event.component.css']
})
export class AdminEventComponent implements OnInit {
  @ViewChild('InputMultiFile') InputMultiFile: ElementRef;
  isFilter = false;
  EventsData: any[] = [];
  multiFileURL: any[] = [];
  multiImagesFile: any[] = [];
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
  UserModel: any;
  EventId: any;
  TotalCount = 0;

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));;
    this.FormInit();
    this.GetAllEvents();
    this.GetWebsiteAdminFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      title: ['', Validators.required],
      description: ['', Validators.required],
      fromDate: null,
      toDate: null,
      month: null,
      isVisible: true,
      insertUser: null
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      id: item.id,
      title: item.title,
      description: item?.description,
      fromDate: item?.fromDate,
      toDate: item?.toDate,
      month: this.datePipe.transform(item?.month, 'yyyy-MM'),
      isVisible: item?.isVisible,
      insertUser: this.UserModel?.userId ?? null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.EventId = '';
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
    this.EventId = item.id;
    this.InputMultiFile.nativeElement.value = '';
    this.adminService.GetEventSliderImagesById(item.id).subscribe(data => {
      this.multiFileURL = data;
      this.modalService.open(content, {
        size: 'xl',
        scrollable: true,
        centered: true
      });
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.EventId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllEvents() {
    this.adminService.GetAllEvents(this.PagingFilter).subscribe(data => {
      this.EventsData = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllEvents();
  }

  GetWebsiteAdminFilters() {
    this.adminService.GetAllWebPagesFilters('Event').subscribe(data => {
      this.FilterList = data;

    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllEvents();
  }

  onMultiFileChange(event: any) {
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

    this.FileModel.id = this.EventId;
    this.FileModel.files = this.multiImagesFile.map(i => i.file);
    const formData = new FormData();
    this.formService.buildFormData(formData, this.FileModel);
    this.adminService.AddEventSliderImage(formData).subscribe(data => {
      if (data.done) {
        this.modalService.dismissAll();
        this.toaster.success(data.message);
      }
      else
        this.toaster.error(data.message);
    });
  }

  AddNewEvent() {
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    if (this.ItemForm.controls['id'].value == 0) {
      this.adminService.AddNewEvent(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllEvents();
          this.GetWebsiteAdminFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    } else {
      this.adminService.UpdateEvent(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllEvents();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    }
  }

  DeleteItem() {
    this.adminService.DeleteEvent(this.EventId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllEvents();
        this.GetWebsiteAdminFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
    });
  }
}
