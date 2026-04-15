import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent {
  UserModel: any;
  FilterList: FilterModel[] = [];
  isFilter = true;
  showLoader = false;
  ItemForm: FormGroup;
  defaultImage = 'balena-2.jpeg';
  Total = 0;
  CategoryId: any;
  ProductId: any;
  OrderId: any;
  Results: any[] = [];
  Categories: any[] = [];
  Products: any[] = [];
  SelectedProducts: any[] = [];
  CustomerData: any;
  VoidReason: any;
  Notes: any;
  ProductPrice = 0;
  TotalValue = 0;
  CategoryName = 'اختر فئة';
  ProductName = 'اختر عنصر';
  CategoryValidation = false;
  ProductValidation = false;
  OrderNumber: any;
  OrderDate: any;
  CategoryPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 500
  }
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  }


  constructor(private modalService: NgbModal, private adminService: AdminService,
    private formService: ValidationFormService, private offcanvasService: NgbOffcanvas,
    private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.GetAllOrders();
    this.GetAllOrderFilters();
    this.FormInit();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      voidReason: ['', [Validators.required, this.formService.noSpaceValidator]],
      voidNotes: null
    });
  }

  FillEditForm(item: any) {
    this.OrderId = item.orderId;
    this.GetOrderDetailsByOrderId();
  }


  openSidePanel(content: any, item: any) {
    this.OrderId = item.orderId;
    this.OrderNumber = item.orderNumber;
    this.OrderDate = item.createdDate;
    this.GetOrderDetailsByOrderId();
    this.offcanvasService.open(content, { position: 'end' });
  }

  openDeleteItemModal(content: any, item: any) {
    this.ItemForm.reset();
    this.OrderId = item.orderId;
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }


  GetOrderDetailsByOrderId() {
    this.showLoader = true;
    this.adminService.GetOrderDetailsByOrderId(this.OrderId).subscribe(data => {
      this.showLoader = false;
      this.Notes = data?.results?.note;
      this.VoidReason = data?.results?.voidReason;
      this.CustomerData = data?.results?.customer;
      this.SelectedProducts = data?.results?.orderDetails;
      this.TotalValue = this.SelectedProducts.reduce((sum, item) => sum + (item.totalValue || 0), 0);
    });
  }

  GetAllOrders() {
    this.showLoader = true;
    this.adminService.GetAllOrders(this.PagingFilter).subscribe(data => {
      this.showLoader = false;
      this.Results = data.results;
    });
  }

  GetAllOrderFilters() {
    this.adminService.GetAllOrderFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data.results;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllOrders();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllOrders();
    this.GetAllOrderFilters();
  }

  getStatusColor(statusId: number) {
    return {
      'finished': statusId == 1,
      'deleted': statusId == 2
    }
  }

  getOrderTypeColor(typeId: number) {
    return {
      'takeaway': typeId == 1,
      'delivery': typeId == 2
    }
  }

  CancelOrder() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.showLoader = true;
    let fromValue = this.ItemForm.value;

    this.adminService.CancelOrder(fromValue.voidReason, fromValue.action, fromValue.voidNotes, this.OrderId).subscribe(data => {
      this.showLoader = false;
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllOrders();
        this.GetAllOrderFilters();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
