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
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.css']
})
export class ExpensesComponent implements OnInit {
  Results: any[] = [];
  ExpensesCategory: any[] = [];
  UserModel: any;
  showLoader = false;
  isFilter = true;
  ItemForm: FormGroup;
  Total = 0;
  ExpensesId: any;
  FilterList: FilterModel[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  }
  formErrors = {
    expenseCategoryId: '',
    amount: '',
    expenseDate: '',
    reason: ''
  };

  constructor(private modalService: NgbModal, private employeeService: EmployeeService,
    private formService: ValidationFormService, private datePipe: DatePipe,
    private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllExpenseCategory();
    this.GetAllExpenses();
    this.GetAllExpensesFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      expensesId: 0,
      expenseCategoryId: ['', [Validators.required, this.formService.noSpaceValidator]],
      amount: ['', [Validators.required]],
      expenseDate: ['', [Validators.required, this.formService.noSpaceValidator]],
      reason: ['', [this.formService.noSpaceValidator]],
      insertUser: null,
    });

    this.ItemForm.valueChanges.subscribe((data) => {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, true);
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      expensesId: item.expensesId,
      expenseCategoryId: item.expenseCategoryId,
      amount: item.amount,
      expenseDate: this.datePipe.transform(item.expenseDate, 'yyyy-MM-dd'),
      reason: item.reason,
      insertUser: this.UserModel?.userId,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('expensesId').setValue(0);
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
    this.ExpensesId = item.expensesId
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllExpenseCategory() {
    this.employeeService.GetAllExpenseCategory().subscribe(data => {
      this.ExpensesCategory = data.results.map(i => { return { id: i.expenseCategoryId, name: i.name } });
    });
  }

  GetAllExpenses() {
    this.showLoader = true;
    this.employeeService.GetAllExpenses(this.PagingFilter).subscribe(data => {
      this.showLoader = false;
      this.Results = data.results;
      this.Total = data.totalCount;
    });
  }

  GetAllExpensesFilters() {
    this.employeeService.GetAllExpensesFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data.results;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllExpenses();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllExpenses();
    this.GetAllExpensesFilters();
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
    if (this.ItemForm.controls['expensesId'].value == 0) {
      this.employeeService.AddNewExpenses(this.ItemForm.value).subscribe(data => {
        this.showLoader = false;
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllExpenses();
          this.GetAllExpensesFilters();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.employeeService.UpdateExpenses(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllExpenses();
          this.GetAllExpensesFilters();
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
    this.employeeService.DeleteExpenses(this.ExpensesId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllExpenses();
        this.GetAllExpensesFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
