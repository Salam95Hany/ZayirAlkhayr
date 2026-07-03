import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { EmployeeService } from 'src/app/Admin/Services/employee.service';

@Component({
  selector: 'app-employee-salary',
  templateUrl: './employee-salary.component.html',
  styleUrls: ['./employee-salary.component.css']
})
export class EmployeeSalaryComponent implements OnInit {
  Results: any[] = [];
  PaidSalaryList: any[] = [];
  UserModel: any;
  SelectedFilter = {} as FilterModel;
  IsCheckedMonthBefore = true;
  IsAllPaid: boolean;
  FilterList: FilterModel[] = [
    {
      categoryName: 'Month',
      categoryDisplayName: 'اختر شهر',
      filterType: 'Month',
      itemId: ''
    }
  ];
  showLoader = false;

  constructor(private employeeService: EmployeeService, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  }

  CheckMonthEmployeePaid() {
    this.showLoader = true;
    this.employeeService.CheckMonthEmployeePaid(this.SelectedFilter.itemId).subscribe(data => {
      this.IsCheckedMonthBefore = data.results;
      if (this.IsCheckedMonthBefore) {
        this.GetEmployeeSalary();
      } else {
        this.showLoader = false;
      }
    });
  }

  GetEmployeeSalary() {
    this.employeeService.GetEmployeeSalary(this.SelectedFilter.itemId).subscribe(data => {
      this.showLoader = false;
      this.Results = data.results;
      this.IsAllPaid = this.Results.every(i => i.status == 2);
      this.Results.forEach(item => {
        if (item.status == 1)
          item.statusName = 'غير مدفوع';
        else
          item.statusName = 'مدفوع';
      });
    });
  }

  CreateMonthEmployeeSalary() {
    if (!this.SelectedFilter.itemId) {
      this.toaster.warning('برجاء اختيار الشهر');
      return;
    }

    this.showLoader = true;
    this.employeeService.CreateMonthEmployeeSalary(this.SelectedFilter.itemId, this.UserModel?.userId).subscribe(data => {
      if (data.isSuccess) {
        this.IsCheckedMonthBefore = true;
        this.GetEmployeeSalary();
      } else {
        this.showLoader = false;
        this.toaster.error(data.message);
      }
    });
  }

  PaidEmployeeSalary(item: any = null) {
    debugger;
    if (item) {
      this.PaidSalaryList = [item];
    } else
      this.PaidSalaryList = [...this.Results.filter(i => i.status == 1)];
    this.showLoader = true;
    this.employeeService.PaidEmployeeSalary(this.PaidSalaryList, this.SelectedFilter.itemId, this.UserModel?.userId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetEmployeeSalary();
      } else
        this.toaster.error(data.message);
    });
  }

  PayrollTransferToExpenses() {
    let totalAmount = this.Results.reduce((sum, item) => sum + item.netSalary, 0);
    this.showLoader = true;
    this.employeeService.PayrollTransferToExpenses(totalAmount, this.SelectedFilter.itemId, this.UserModel?.userId).subscribe(data => {
      this.showLoader = false;
      if (data.isSuccess) {
        this.toaster.success(data.message);
      } else
        this.toaster.error(data.message);
    });
  }


  FilterChecked(filterList: FilterModel[]) {
    if (filterList.length > 0) {
      this.SelectedFilter = filterList[0];
      if (!this.validateSalaryMonth()) {
        return;
      }
      this.Results = [];
      this.IsAllPaid = false;
      this.CheckMonthEmployeePaid();
    } else {
      this.SelectedFilter = {} as FilterModel;
    }
  }

  preventNegative(event: KeyboardEvent): void {
    if (event.key === '-' || event.key === 'e' || event.key === '+') {
      event.preventDefault();
    }
  }

  calculateNetSalary(item: any): void {
    const basicSalary = Number(item.basicSalary) || 0;
    const bonus = Number(item.bonus) || 0;
    const deduction = Number(item.deduction) || 0;
    const advance = Number(item.advance) || 0;

    item.netSalary = basicSalary + bonus - deduction - advance;
  }

  validateSalaryMonth(): boolean {
    if (!this.SelectedFilter.itemId) {
      return false;
    }

    const selectedDate = new Date(this.SelectedFilter.itemId);
    const currentDate = new Date();
    const currentMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    const selectedMonth = new Date(selectedDate.getFullYear(), selectedDate.getMonth(), 1);
    if (selectedMonth > currentMonth) {
      this.toaster.warning('لا يمكن اختيار شهر أكبر من الشهر الحالي.');
      return false;
    }

    return true;
  }
}
