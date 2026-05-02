import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-supplier-payment-log',
  templateUrl: './supplier-payment-log.component.html',
  styleUrls: ['./supplier-payment-log.component.css']
})
export class SupplierPaymentLogComponent implements OnInit {
  UserModel: any;
  isFilter = true;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  ContactableSuppliers = 0;
  SearchText = '';
  PurchaseNumbersList: any[] = [];
  RemainingAmount = 0;
  PurchaseNumber = 'رقم الفاتورة';
  SupplierName = '';
  SupplierId: any;
  SupplierPaymentId: any;
  Results: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryName: 'SearchText',
      categoryDisplayName: 'رقم الفاتورة, رقم السند',
      filterType: 'SearchText',
      itemId: ''
    },
    {
      categoryName: 'DateRange',
      categoryDisplayName: 'التاريخ',
      filterType: 'DateRange',
      itemId: ''
    }
  ];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
  };

  constructor(
    private modalService: NgbModal,
    private inventoryService: InventoryService,
    private formService: ValidationFormService,
    private fb: FormBuilder,
    private toaster: ToastrService,
    private route: ActivatedRoute,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void {
    this.SupplierId = this.route.snapshot.queryParamMap.get('supplierId');
    this.PagingFilter.supplierId = this.SupplierId;
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.FormInit();
    this.GetSupplierNameById();
    this.GetSupplierPaymentLogData();
    this.GetPurchaseNumberBySupplierId();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      supplierPaymentId: 0,
      supplierName: [{ value: this.SupplierName, disabled: true }],
      supplierId: this.SupplierId,
      remainingAmount: [{ value: 0, disabled: true }],
      purchaseId: ['', [Validators.required]],
      amountPaid: ['', [Validators.required, this.formService.noSpaceValidator]],
      paymentDate: ['', [Validators.required]],
      notes: null,
      insertUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.PurchaseNumber = 'رقم الفاتورة';
    this.ItemForm.patchValue({
      supplierPaymentId: 0,
      remainingAmount: 0,
      supplierName: this.SupplierName,
      supplierId: this.SupplierId,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
    });
  }

  FillEditForm(item: any) {
    this.PurchaseNumber = item.purchaseNumber;
    this.ItemForm.setValue({
      supplierPaymentId: item.supplierPaymentId,
      supplierName: this.SupplierName,
      supplierId: this.SupplierId,
      remainingAmount: this.PurchaseNumbersList.find(i => i.purchaseNumber == item.purchaseNumber)?.remainingAmount ?? 0,
      purchaseId: item.purchaseId,
      amountPaid: item.amountPaid,
      paymentDate: this.datePipe.transform(item.paymentDate, 'yyyy-MM-dd'),
      notes: item.notes,
      insertUser: this.UserModel?.userId
    });
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item) {
      this.FillEditForm(item);
    }

    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.SupplierPaymentId = item.supplierPaymentId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetPurchaseNumberBySupplierId() {
    this.inventoryService.GetPurchaseNumberBySupplierId(this.SupplierId).subscribe(data => {
      this.PurchaseNumbersList = data.results;
    })
  }

  GetSupplierNameById() {
    this.inventoryService.GetSupplierNameById(this.SupplierId).subscribe(data => {
      this.SupplierName = data.results;
    })
  }

  GetSupplierPaymentLogData() {
    this.showLoader = true;
    this.inventoryService.GetSupplierPaymentLogData(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.Total = data.totalCount || 0;
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل الموردين');
      }
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetSupplierPaymentLogData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetSupplierPaymentLogData();
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }

  onPurchaseNumberClicked(item: any) {
    this.PurchaseNumber = item.purchaseNumber;
    this.ItemForm.patchValue({ purchaseId: item.purchaseId, remainingAmount: item.remainingAmount });

  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    if (!this.ItemForm.valid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    const payload = {
      ...this.ItemForm.value,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
    };
    debugger
    if (+payload.amountPaid > +this.ItemForm.controls["remainingAmount"].value) {
      this.toaster.warning('لا يمكن اضافة مبلغ أكبر من الباقي');
      return;
    }

    this.showLoader = true;
    if (payload.supplierPaymentId === 0) {
      this.inventoryService.AddNewSupplierPayment(payload).subscribe(data => {
        this.HandleSaveResponse(data);
      });
      return;
    }

    this.inventoryService.UpdateSupplierPayment(payload).subscribe(data => {
      this.HandleSaveResponse(data);
    });
  }

  DeleteItem() {
    if (!this.SupplierId) {
      return;
    }

    this.showLoader = true;
    this.inventoryService.DeleteSupplierPayment(this.SupplierPaymentId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message || '');
        this.GetSupplierPaymentLogData();
        this.GetPurchaseNumberBySupplierId();
        this.modalService.dismissAll();
      } else {
        this.toaster.error(data.message || '');
      }
      this.showLoader = false;
    });
  }

  private HandleSaveResponse(data: any) {
    if (data.isSuccess) {
      this.toaster.success(data.message || '');
      this.GetSupplierPaymentLogData();
      this.GetPurchaseNumberBySupplierId();
      this.modalService.dismissAll();
    } else {
      this.toaster.error(data.message || '');
    }

    this.showLoader = false;
  }
}
