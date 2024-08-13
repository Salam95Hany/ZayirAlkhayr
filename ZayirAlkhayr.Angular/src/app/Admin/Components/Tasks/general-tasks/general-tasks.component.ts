import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-general-tasks',
  templateUrl: './general-tasks.component.html',
  styleUrls: ['./general-tasks.component.css']
})
export class GeneralTasksComponent implements OnInit {
  TasksData: any[] = [];
  UsersData: any[] = [];
  UserModel: any;
  ItemForm: FormGroup;
  TotalCount = 0;
  isFilter = false;
  showLoader = false;
  UserName = '';
  UserId: any;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  };

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllUsers();
    this.GetAllGeneralTasks();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      statusId: 0,
      task: ['', [Validators.required, this.formService.noSpaceValidator]],
      assignTo: null,
      InsertUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
    this.ItemForm.get('statusId').setValue(0);
  }

  openAddItemModal(content: any) {
    this.ResetForm();
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  GetAllGeneralTasks() {
    // this.adminService.GetAllGeneralTasks(this.PagingFilter).subscribe(data => {
    //   this.TasksData = data;
    //   this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    // });
  }

  GetAllUsers() {
    this.adminService.GetAllUsers().subscribe(data => {
      this.UsersData = data;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllGeneralTasks();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllGeneralTasks();
  }

  UserChange(item: any) {

  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.showLoader = true;
    this.adminService.AddNewBeneFactorType(this.ItemForm.value).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllGeneralTasks();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

}
