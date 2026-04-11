import { Component, Inject } from '@angular/core';
import { PagingFilterModel } from '../../Models/PagingFilterModel';
import { AdminService } from '../../Services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationFormService } from '../../Services/validation-form.service';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css']
})
export class CreateOrderComponent {
  orderModel = {} as any;
  foodItemsList: any[] = [];
  selectedFoodItems: any[] = [];
  addSelectedFoodItem: any;
  categoriesList: any[] = [];
  showLoader: boolean = false;
  isOrderCatOpen = false;
  fullscreenMode = false;
  activeCat = null;
  OrderId: any;
  NoteTxt = '';
  defaultImage = '../../../../assets/PosLogo.jpeg';
  keys: any[] = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];
  UserModel: any;
  cashAmount = '';
  amountPaid = '';
  remaining = 0;
  CurrentTime: any;
  ele: any;
  CostDelivery = 10000;
  QtyFoodItemInputCounter: any;
  OrderTypeId = 1;
  CustomerSearch = '';
  CustomerSearchData: any;
  ItemCustomerForm: FormGroup;
  CategoryPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 100
  }


  constructor(private adminService: AdminService, private toaster: ToastrService, private modalService: NgbModal, private route: ActivatedRoute,
    @Inject(DOCUMENT) private document: any, private fb: FormBuilder, private formService: ValidationFormService, private offcanvasService: NgbOffcanvas
  ) { }

  ngOnInit(): void {
    this.ele = document.documentElement;
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.OrderId = this.route.snapshot.queryParamMap.get('orderId');
    this.orderModel.totalValue = 0;
    this.GetCurrentTime();
    this.GetAllCategories();
    this.CustomerFormInit();
    if (this.OrderId)
      this.GetOrderWithDetailsByOrderId();
  }

  CustomerFormInit() {
    this.ItemCustomerForm = this.fb.group({
      customerId: 0,
      fullName: ['', [Validators.required, this.formService.noSpaceValidator]],
      phone: ['', [Validators.required, this.formService.noSpaceValidator]],
      address: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null
    });
  }

  GetOrderWithDetailsByOrderId() {
    this.showLoader = true;
    this.adminService.GetOrderWithDetailsByOrderId(this.OrderId).subscribe(data => {
      this.showLoader = false;
      if (data.isSuccess) {
        this.NoteTxt = data?.results?.notes;
        this.orderModel.notes = data?.results?.notes;
        this.orderModel.totalValue = data?.results?.totalValue;
        this.CustomerSearchData = data?.results?.customer;
        this.OrderTypeId = data?.results?.orderType;
        this.selectedFoodItems = data.results.orderDetails;
      } else {
        this.toaster.error(data.message);
      }
    });
  }

  openExpandModal(content: any) {
    this.modalService.open(content, { size: 'lg', centered: true, scrollable: true })
  }

  openNotesModal(content: any) {
    this.modalService.open(content, { size: 'lg', centered: true, scrollable: true })
  }

  openPayModal(content: any) {
    this.amountPaid = '';
    this.cashAmount = '';
    this.remaining = 0;
    this.modalService.open(content, { size: 'md', centered: true, scrollable: true });
  }

  OpenOrderTypeModal(content: any) {
    this.modalService.open(content, { size: 'lg', centered: true, scrollable: true });
  }

  OpenCustomerSearchModal(content: any) {
    this.CustomerSearch = '';
    this.modalService.open(content, { size: 'lg', centered: true, scrollable: true });
  }

  OpenAddNewCustomerModal(content: any) {
    this.CustomerSearch = '';
    this.modalService.open(content, { size: 'lg', centered: true, scrollable: true });
  }

  openHistorySidePanel(content) {
    if (!this.CustomerSearchData?.customerId) {
      this.toaster.warning('برجاء اختيار عميل');
      return;
    }

    this.getCustomerOrdersHistory();
    this.offcanvasService.open(content, { position: 'end' });
  }
  customerOrdersHistory: any[] = [];
  getCustomerOrdersHistory() {
    if (this.CustomerSearchData) {
      this.adminService.GetCustomerOrdersHistory(this.CustomerSearchData?.customerId).subscribe(data => {
        this.customerOrdersHistory = data.results;
        this.customerOrdersHistory.map(a => a.isCollapsed = false);
      });
    }
  }

  getOrderDetailsFromArchieve(index: number) {
    this.customerOrdersHistory.map((item, i) => {
      if (index == i) {
        item.isCollapsed = !item.isCollapsed
      } else {
        item.isCollapsed = false;
      }
    });
  }

  onClickFoodItem(item: any, content: any) {
    if (item.offerPrice != null)
      item.price = item.offerPrice;

    this.addSelectedFoodItem = item;
    this.addSelectedFoodItem.masterQuantity = 1
    this.modalService.open(content, { size: 'md', centered: true, scrollable: true });
  }

  GetCurrentTime() {
    let intervalClock = setInterval(() => {
      let Time = new Date();
      this.CurrentTime = Time.getHours() + ':' + (Time.getMinutes() < 10 ? '0' : '') + Time.getMinutes()
    }, 1000);
  }

  GetAllCategories() {
    this.showLoader = true;
    this.adminService.GetAllCategories(this.CategoryPagingFilter).subscribe((data) => {
      this.categoriesList = data.results;
      this.showLoader = false;
    });
  }

  GetProductsByCategoryId(CategoryId: any) {
    this.adminService.GetProductsByCategoryId(CategoryId).subscribe(data => {
      this.foodItemsList = data.results;
    });
  }

  changeMasterItemQuantity(quantity: number, item: any) {
    item.masterQuantity = parseInt(item.masterQuantity) + quantity;
    if (item.masterQuantity == 0) {
      item.masterQuantity = 1;
    }
  }

  GetItemQuantityNumbers(key: any) {
    if (key == 'C') {
      this.addSelectedFoodItem.masterQuantity = '1';
      this.QtyFoodItemInputCounter = 0;
    }
    else {
      if (this.QtyFoodItemInputCounter == 0)
        this.addSelectedFoodItem.masterQuantity = key;
      else
        this.addSelectedFoodItem.masterQuantity = this.addSelectedFoodItem.masterQuantity + key;

      this.QtyFoodItemInputCounter = this.QtyFoodItemInputCounter + 1;
    }
  }

  createMasterItems() {
    if (this.addSelectedFoodItem.masterQuantity && this.addSelectedFoodItem.masterQuantity > 0) {
      let obj = {
        productId: this.addSelectedFoodItem.itemId,
        productName: this.addSelectedFoodItem.name,
        image: this.addSelectedFoodItem.image,
        quantity: this.addSelectedFoodItem.masterQuantity,
        price: this.addSelectedFoodItem.price,
        totalValue: this.addSelectedFoodItem.price * Number(this.addSelectedFoodItem.masterQuantity)
      };
      this.selectedFoodItems.push(obj);
      this.calculateOrderSummary();
      this.modalService.dismissAll();
    }
    else
      this.toaster.error('Please Insert Quantity');
  }

  removeItem(index: number) {
    this.selectedFoodItems.splice(index, 1);
    this.calculateOrderSummary();
  }

  saveOrderNotes() {
    this.orderModel.notes = this.NoteTxt;
  }

  clearAllNotes() {
    this.NoteTxt = '';
  }

  calculateOrderSummary() {
    this.orderModel.totalValue = 0;
    this.selectedFoodItems.map(item => {
      this.orderModel.totalValue += item.totalValue;
    });
  }

  onAmountPaidChange() {
    if (this.cashAmount)
      this.remaining = Number(this.cashAmount) - this.orderModel.totalValue;
    else
      this.remaining = 0;
  }

  NumbersOnly(key: any): boolean {
    let patt = /^([0-9\+])$/;
    let result = patt.test(key);
    return result;
  }

  GetPayInputNumbers(key: any) {
    if (key == 'C')
      this.cashAmount = '';
    else
      this.cashAmount = this.cashAmount + key;
    this.onAmountPaidChange()
  }

  getTableNumberSelected(tableNumber: any) {
    this.orderModel.tableNumber = tableNumber;
    this.modalService.dismissAll();
  }

  toggleFullscreenWindow() {
    this.fullscreenMode = !this.fullscreenMode;
    if (this.fullscreenMode) {
      if (this.ele.requestFullscreen) {
        this.ele.requestFullscreen();
      } else if (this.ele.mozRequestFullScreen) {
        this.ele.mozRequestFullScreen();
      } else if (this.ele.webkitRequestFullscreen) {
        this.ele.webkitRequestFullscreen();
      } else if (this.ele.msRequestFullscreen) {
        this.ele.msRequestFullscreen();
      }
    } else {
      if (this.document.exitFullscreen) {
        this.document.exitFullscreen();
      } else if (this.document.mozCancelFullScreen) {
        this.document.mozCancelFullScreen();
      } else if (this.document.webkitExitFullscreen) {
        this.document.webkitExitFullscreen();
      } else if (this.document.msExitFullscreen) {
        this.document.msExitFullscreen();
      }
    }
  }

  resetOrderModel() {
    this.orderModel = {};
    this.selectedFoodItems = [];
    this.foodItemsList = [];
    this.orderModel.totalValue = 0;
    this.OrderId = null;
    this.cashAmount = '';
    this.amountPaid = '';
    this.NoteTxt = '';
    this.remaining = 0;
    this.ResetCustomer();
    this.OrderTypeId = 1;
  }

  createOrder() {
    if (this.selectedFoodItems.length == 0) {
      this.toaster.warning('برجاء اضافة عنصر واحد على الأقل');
      return;
    }

    if (this.OrderTypeId == 2 && !this.CustomerSearchData?.customerId) {
      this.toaster.warning('برجاء اختيار عميل');
      return;
    }

    this.showLoader = true;
    this.orderModel.customerId = this.CustomerSearchData?.customerId ?? null;
    this.orderModel.totalAmount = this.orderModel.totalValue;
    this.orderModel.orderType = this.OrderTypeId;
    this.orderModel.costDelivery = this.OrderTypeId == 1 ? 0 : this.CostDelivery;
    this.orderModel.note = this.NoteTxt;
    this.orderModel.userId = this.UserModel?.userId;
    this.orderModel.details = this.selectedFoodItems.map(i => {
      return {
        ProductID: i.productId,
        quantity: i.quantity,
        unitPrice: i.totalValue,
      }
    });

    if (!this.OrderId) {
      this.adminService.AddNewOrder(this.orderModel).subscribe(data => {
        this.showLoader = false;
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.resetOrderModel();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    } else {
      this.orderModel.orderId = this.OrderId;
      this.adminService.UpdateOrder(this.orderModel).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.resetOrderModel();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    }
  }

  CustomerSearchId: number;
  OnCustomerSearch(content: any) {
    if (!this.CustomerSearch) {
      this.toaster.warning('برجاء إدخال رقم العميل');
      return;
    }

    if (this.CustomerSearch.length < 9) {
      this.toaster.warning('برجاء إدخال رقم هاتف صحيح');
      return;
    }

    if (this.CustomerSearch.length > 3) {
      this.adminService.GetCustomerByPhone(this.CustomerSearch).subscribe(data => {
        if (data.results.length > 0) {
          this.CustomerSearchData = data.results[0];
          this.modalService.dismissAll();
        } else {
          this.CustomerSearchData = null;
          this.modalService.dismissAll();
          this.modalService.open(content, { size: 'lg', centered: true, scrollable: true });
        }
      });
    }
  }

  GetCustomerById() {
    this.adminService.GetCustomerById(this.CustomerSearchId).subscribe(data => {
      this.CustomerSearchData = data.results;
    })
  }

  AddNewCustomerItem() {
    this.ItemCustomerForm = this.formService.TrimFormInputValue(this.ItemCustomerForm);
    let isValid = this.ItemCustomerForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemCustomerForm);
      return;
    }

    this.showLoader = true;
    this.adminService.AddNewCustomer(this.ItemCustomerForm.value).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.CustomerSearchId = data.results;
        this.GetCustomerById();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  ResetCustomer() {
    this.CustomerSearch = '';
    this.CustomerSearchData = null;
    this.CustomerSearchId = null;
  }
}
