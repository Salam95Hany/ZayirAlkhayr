import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent {
  UserModel: any;
  isFilter = true;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  CustomerId: any;
  Results: any[] = [];
  // [PlaceHolder]="'بالاسم, رقم التلفون'"
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
    this.GetAllCustomers();
    this.FormInit();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      customerId: 0,
      fullName: ['', [Validators.required, this.formService.noSpaceValidator]],
      phone: ['', [Validators.required, this.formService.noSpaceValidator]],
      address: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      customerId: item.customerId,
      fullName: item.name,
      phone: item.phone,
      address: item.address,
      insertUser: this.UserModel?.userId
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('customerId').setValue(0);
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item)
      this.FillEditForm(item);

    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.CustomerId = item.customerId
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllCustomers() {
    this.adminService.GetAllCustomers(this.PagingFilter).subscribe(data => {
      this.Results = data.results;
      this.Total = data.totalCount;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllCustomers();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
     this.GetAllCustomers();
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    this.showLoader = true;
    if (this.ItemForm.controls['customerId'].value == 0) {
      this.adminService.AddNewCustomer(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllCustomers();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.adminService.UpdateCustomer(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllCustomers();
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
    this.adminService.DeleteCustomer(this.CustomerId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllCustomers();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
