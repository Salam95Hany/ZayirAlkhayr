import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { EmployeeService } from 'src/app/Admin/Services/employee.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {
  Results: any[] = [];
  JoipTitle: any[] = [];
  UserModel: any;
  showLoader = false;
  isFilter = true;
  ItemForm: FormGroup;
  Total = 0;
  EmployeeId: any;
  FilterList: FilterModel[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  }
  formErrors = {
    jobTitleId: '',
    fullName: '',
    phoneNumber: '',
    basicSalary: '',
    hireDate: '',
    notes: ''
  };

  constructor(private modalService: NgbModal, private employeeService: EmployeeService,
    private formService: ValidationFormService,private datePipe: DatePipe,
    private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllJobTitle();
    this.GetAllEmployees();
    this.GetAllEmployeeFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      employeeId: 0,
      jobTitleId: ['', [Validators.required, this.formService.noSpaceValidator]],
      fullName: ['', [Validators.required, this.formService.noSpaceValidator]],
      phoneNumber: ['', [Validators.required, this.formService.noSpaceValidator]],
      basicSalary: ['', [Validators.required]],
      hireDate: ['', [Validators.required]],
      notes: ['', [this.formService.noSpaceValidator]],
      insertUser: null,
    });

    this.ItemForm.valueChanges.subscribe((data) => {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, true);
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      employeeId: item.employeeId,
      jobTitleId: item.jobTitleId,
      fullName: item.fullName,
      phoneNumber: item.phoneNumber,
      basicSalary: item.basicSalary,
      hireDate: this.datePipe.transform(item.hireDate, 'yyyy-MM-dd'),
      notes: item.notes,
      insertUser: this.UserModel?.userId,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('employeeId').setValue(0);
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

  openDeleteItemModal(content: any, item: any) {
    this.EmployeeId = item.employeeId
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllJobTitle() {
    this.employeeService.GetAllJobTitle().subscribe(data => {
      this.JoipTitle = data.results.map(i => { return { id: i.jobTitleId, name: i.name } });
    });
  }

  GetAllEmployees() {
    this.showLoader = true;
    this.employeeService.GetAllEmployees(this.PagingFilter).subscribe(data => {
      this.showLoader = false;
      this.Results = data.results;
      this.Total = data.totalCount;
    });
  }

  GetAllEmployeeFilters() {
    this.employeeService.GetAllEmployeeFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data.results;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllEmployees();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllEmployees();
  }

  validateForm(): boolean {
    this.formService.markFormGroupTouched(this.ItemForm);
    if (this.ItemForm.valid) {
      return true;
    } else {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, false)
      return false;
    }
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.validateForm();
    if (!isValid)
      return;

    this.showLoader = true;
    if (this.ItemForm.controls['employeeId'].value == 0) {
      this.employeeService.AddNewEmployee(this.ItemForm.value).subscribe(data => {
        this.showLoader = false;
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllEmployees();
          this.GetAllEmployeeFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.employeeService.UpdateEmployee(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllEmployees();
          this.GetAllEmployeeFilters();
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
    this.employeeService.DeleteEmployee(this.EmployeeId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllEmployees();
        this.GetAllEmployeeFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
