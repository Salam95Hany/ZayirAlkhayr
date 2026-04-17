import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html',
  styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent {
  UserModel: any;
  isFilter = false;
  showLoader = false;
  PurchaseForm: FormGroup;
  Total = 0;
  CurrentPageAmount = 0;
  PurchaseId: number | null = null;
  Results: any[] = [];
  Suppliers: any[] = [];
  InventoryItems: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryName: 'SearchText',
      categoryDisplayName: 'المشتريات',
      filterType: 'SearchText',
      itemId: ''
    },
    {
      categoryName: 'DateRange',
      categoryDisplayName: 'الفترة الزمنية',
      filterType: 'DateRange'
    }
  ];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
  };
  SupplierPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 1000
  };
  InventoryItemsPagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 1000
  };

  constructor(
    private modalService: NgbModal,
    private inventoryService: InventoryService,
    private formService: ValidationFormService,
    private fb: FormBuilder,
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.FormInit();
    this.GetAllPurchases();
    this.GetSuppliers();
    this.GetInventoryItems();
  }

  get PurchaseItemsFormArray(): FormArray {
    return this.PurchaseForm.get('items') as FormArray;
  }

  FormInit() {
    this.PurchaseForm = this.fb.group({
      purchaseId: 0,
      supplierId: [null, [Validators.required]],
      userId: this.UserModel?.userId,
      items: this.fb.array([this.CreatePurchaseItemForm()])
    });
  }

  CreatePurchaseItemForm(item: any = null): FormGroup {
    return this.fb.group({
      inventoryItemId: [item?.inventoryItemId ?? null, [Validators.required]],
      quantity: [item?.quantity ?? 1, [Validators.required, Validators.min(1)]],
      costPrice: [item?.costPrice ?? 0, [Validators.required, Validators.min(0)]]
    });
  }

  ResetForm() {
    this.PurchaseForm.reset();
    this.PurchaseForm.setControl('items', this.fb.array([this.CreatePurchaseItemForm()]));
    this.PurchaseForm.patchValue({
      purchaseId: 0,
      supplierId: null,
      userId: this.UserModel?.userId
    });
  }

  FillEditForm(item: any) {
    this.PurchaseForm.patchValue({
      purchaseId: item.purchaseId,
      supplierId: item.supplierId,
      userId: this.UserModel?.userId
    });

    const itemsArray = this.fb.array(
      (item.items || []).map((purchaseItem: any) => this.CreatePurchaseItemForm(purchaseItem))
    );

    this.PurchaseForm.setControl('items', itemsArray.length > 0 ? itemsArray : this.fb.array([this.CreatePurchaseItemForm()]));
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (!item) {
      this.modalService.open(content, {
        size: 'xl',
        scrollable: true,
        centered: true
      });
      return;
    }

    this.showLoader = true;
    this.inventoryService.GetPurchaseById(item.purchaseId).subscribe({
      next: (data) => {
        if (data.isSuccess && data.results) {
          this.FillEditForm(data.results);
          this.modalService.open(content, {
            size: 'xl',
            scrollable: true,
            centered: true
          });
        } else {
          this.toaster.error(data.message || '');
        }
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل تفاصيل عملية الشراء');
      }
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.PurchaseId = item.purchaseId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetSuppliers() {
    this.inventoryService.GetAllSuppliers(this.SupplierPagingFilter).subscribe({
      next: (data) => this.Suppliers = data.results || [],
      error: () => this.Suppliers = []
    });
  }

  GetInventoryItems() {
    this.inventoryService.GetAllInventoryItems(this.InventoryItemsPagingFilter).subscribe({
      next: (data) => this.InventoryItems = data.results || [],
      error: () => this.InventoryItems = []
    });
  }

  GetAllPurchases() {
    this.showLoader = true;
    this.inventoryService.GetAllPurchases(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.Total = data.totalCount || 0;
        this.CurrentPageAmount = this.Results.reduce((sum, item) => sum + (+item.totalAmount || 0), 0);
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل المشتريات');
      }
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllPurchases();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllPurchases();
  }

  onSupplierClicked(item: any) {
    this.PurchaseForm.patchValue({ supplierId: item.supplierId });
    this.PurchaseForm.get('supplierId')?.markAsTouched();
  }

  onInventoryItemClicked(index: number, item: any) {
    this.GetPurchaseItemGroup(index).patchValue({ inventoryItemId: item.inventoryItemId });
    this.GetPurchaseItemGroup(index).get('inventoryItemId')?.markAsTouched();
  }

  AddPurchaseLine() {
    this.PurchaseItemsFormArray.push(this.CreatePurchaseItemForm());
  }

  RemovePurchaseLine(index: number) {
    if (this.PurchaseItemsFormArray.length === 1) {
      this.PurchaseItemsFormArray.at(0).reset({
        inventoryItemId: null,
        quantity: 1,
        costPrice: 0
      });
      return;
    }

    this.PurchaseItemsFormArray.removeAt(index);
  }

  GetPurchaseItemGroup(index: number): FormGroup {
    return this.PurchaseItemsFormArray.at(index) as FormGroup;
  }

  GetSupplierName(id: number): string {
    return this.Suppliers.find(i => i.supplierId === id)?.name || 'اختر المورد';
  }

  GetInventoryItemName(id: number): string {
    return this.InventoryItems.find(i => i.inventoryItemId === id)?.name || 'اختر عنصر المخزون';
  }

  GetPurchaseItemTotal(index: number): number {
    const group = this.GetPurchaseItemGroup(index);
    const quantity = +group.get('quantity')?.value || 0;
    const costPrice = +group.get('costPrice')?.value || 0;
    return quantity * costPrice;
  }

  GetPurchaseTotal(): number {
    return this.PurchaseItemsFormArray.controls.reduce((sum, _, index) => sum + this.GetPurchaseItemTotal(index), 0);
  }

  AddNewItem() {
    this.PurchaseForm = this.formService.TrimFormInputValue(this.PurchaseForm);
    this.PurchaseForm.markAllAsTouched();
    this.PurchaseItemsFormArray.controls.forEach(control => control.markAllAsTouched());

    if (!this.PurchaseForm.valid || this.PurchaseItemsFormArray.length === 0) {
      return;
    }

    const items = this.PurchaseItemsFormArray.controls.map(control => ({
      inventoryItemId: +control.get('inventoryItemId')?.value,
      quantity: +control.get('quantity')?.value,
      costPrice: +control.get('costPrice')?.value
    }));

    const hasInvalidItems = items.some(item => !item.inventoryItemId || item.quantity <= 0 || item.costPrice < 0);
    if (hasInvalidItems) {
      this.toaster.warning('يرجى استكمال بيانات جميع أصناف الشراء بشكل صحيح');
      return;
    }

    const payload = {
      purchaseId: +this.PurchaseForm.value.purchaseId || null,
      supplierId: +this.PurchaseForm.value.supplierId,
      userId: this.UserModel?.userId,
      items
    };

    this.showLoader = true;
    if (!payload.purchaseId) {
      this.inventoryService.AddPurchase(payload).subscribe(data => {
        this.HandleSaveResponse(data);
      });
      return;
    }

    this.inventoryService.UpdatePurchase(payload).subscribe(data => {
      this.HandleSaveResponse(data);
    });
  }

  DeleteItem() {
    if (!this.PurchaseId) {
      return;
    }

    this.showLoader = true;
    this.inventoryService.DeletePurchase(this.PurchaseId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message || '');
        this.GetAllPurchases();
        this.GetInventoryItems();
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
      this.GetAllPurchases();
      this.GetInventoryItems();
      this.modalService.dismissAll();
    } else {
      this.toaster.error(data.message || '');
    }

    this.showLoader = false;
  }
}
