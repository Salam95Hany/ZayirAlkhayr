import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-general-tasks',
  templateUrl: './general-tasks.component.html',
  styleUrls: ['./general-tasks.component.css']
})
export class GeneralTasksComponent implements OnInit {
  TasksData: any[] = [];
  UsersData: any[] = [];
  FilterList: FilterModel[] = [];
  UserModel: any;
  ItemForm: FormGroup;
  TotalCount = 0;
  isFilter = false;
  showLoader = false;
  UserName = 'تعيين ل';
  UserId: any;
  TaskId: any
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };

  constructor(private modalService: NgbModal, private adminService: AdminService, private formService: ValidationFormService
    , private adminWebsiteService: AdminWebsiteService, private fb: FormBuilder, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllUsers();
    this.GetAllGeneralTasksData();
    this.GetAllGeneralTasksFilter();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      task: ['', [Validators.required, this.formService.noSpaceValidator]],
      assignTo: null,
      InsertUser: null
    });
  }

  FillEditForm(item: any) {
    debugger;
    this.UserId = item.assignToId;
    this.UserName = item.assignTo ? item.assignTo : 'تعيين ل';
    this.ItemForm.setValue({
      id: item.id,
      task: item.task,
      assignTo: item?.assignToId,
      InsertUser: this.UserModel?.userId,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.UserId = '';
    this.UserName = 'تعيين ل';
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
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

  openDeleteItemModal(content: any, item: any) {
    this.TaskId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllGeneralTasksData() {
    this.adminService.GetAllGeneralTasksData(this.PagingFilter).subscribe(data => {
      this.TasksData = data;
      this.TasksData.forEach(i => {
        i.showButtonStatus = this.UserModel?.userId == i.assignToId;
      })
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  GetAllGeneralTasksFilter() {
    this.adminService.GetAllGeneralTasksFilter(this.PagingFilter).subscribe(data => {
      this.FilterList = data;
    });
  }

  GetAllUsers() {
    this.adminWebsiteService.GetAllUsers().subscribe(data => {
      this.UsersData = data;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllGeneralTasksData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllGeneralTasksData();
  }

  UserChange(item: any) {
    debugger;
    this.UserId = item.userId;
    this.UserName = item.userName;
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    if (!this.UserName.startsWith('تعيين ل'))
      this.ItemForm.patchValue({ assignTo: this.UserId });

    if (this.ItemForm.controls['id'].value == 0) {
      this.showLoader = true;
      this.adminService.AddNewGeneralTask(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllGeneralTasksData();
          this.GetAllGeneralTasksFilter();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.showLoader = true;
      this.adminService.UpdateGeneralTask(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllGeneralTasksData();
          this.GetAllGeneralTasksFilter();
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
    this.adminService.DeleteGeneralTask(this.TaskId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllGeneralTasksData();
        this.GetAllGeneralTasksFilter();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  ConvertTaskStatus(taskId: any, statusId: any) {
    this.showLoader = true;
    this.adminService.ConvertTaskStatus(taskId, statusId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllGeneralTasksData();
        this.GetAllGeneralTasksFilter();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

}
