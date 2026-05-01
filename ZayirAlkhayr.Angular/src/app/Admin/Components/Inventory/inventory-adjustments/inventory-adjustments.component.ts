import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-inventory-adjustments',
  templateUrl: './inventory-adjustments.component.html',
  styleUrls: ['./inventory-adjustments.component.css']
})
export class InventoryAdjustmentsComponent {
  UserModel: any;
  isFilter = true;
  showLoader = false;
  SearchText = '';
  ItemForm: FormGroup;
  AdjDetails: any[] = [];
  ActionId = '';
  CreatedDate: any;
  Total = 0;
  NetQuantityChange = 0;
  InventoryAdjustmentId: number | null = null;
  Results: any[] = [];
  InventoryItems: any[] = [];
  FilterList: FilterModel[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
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
    private toaster: ToastrService,
    private offcanvasService: NgbOffcanvas
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.FormInit();
    this.GetAllInventoryAdjustmentData();
    this.GetAllInventoryAdjustmentFilters();
    this.GetInventoryItems();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      inventoryAdjustmentId: 0,
      inventoryItemId: [null, [Validators.required]],
      quantityChange: [null, [Validators.required]],
      reason: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null,
      updateUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.patchValue({
      inventoryAdjustmentId: 0,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      inventoryAdjustmentId: item.inventoryAdjustmentId,
      inventoryItemId: item.inventoryItemId,
      quantityChange: item.quantityChange,
      reason: item.reason,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
    });
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item) {
      this.GetInventoryAdjustmentById(item.inventoryAdjustmentId);
    }

    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  openSidePanel(content: any, item: any) {
    this.ActionId = item.actionId;
    this.CreatedDate = item.insertDate;
    this.GetAdjustmentDetailsById(item.inventoryAdjustmentId);
    this.offcanvasService.open(content, { position: 'end' });
  }

  GetAdjustmentDetailsById(inventoryAdjustmentId: number) {
    this.showLoader = true;
    this.inventoryService.GetAdjustmentDetailsById(inventoryAdjustmentId).subscribe({
      next: (data) => {
        this.AdjDetails = data.results;
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
      }
    });
  }

  GetInventoryAdjustmentById(inventoryAdjustmentId: number) {
    this.showLoader = true;
    this.inventoryService.GetInventoryAdjustmentById(inventoryAdjustmentId).subscribe(data => {
      this.FillEditForm(data.results);
      this.showLoader = false;
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.InventoryAdjustmentId = item.inventoryAdjustmentId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetInventoryItems() {
    this.inventoryService.GetAllInventoryItems(this.InventoryItemsPagingFilter).subscribe({
      next: (data) => this.InventoryItems = data.results || [],
      error: () => this.InventoryItems = []
    });
  }

  GetAllInventoryAdjustmentData() {
    this.showLoader = true;
    this.inventoryService.GetAllInventoryAdjustmentData(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.Total = data.totalCount || 0;
        this.NetQuantityChange = this.Results.reduce((sum, item) => sum + (+item.quantityChange || 0), 0);
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل حركات المخزون');
      }
    });
  }

  GetAllInventoryAdjustmentFilters() {
    this.inventoryService.GetAllInventoryAdjustmentFilters(this.PagingFilter).subscribe({
      next: (data) => {
        this.FilterList = data.results;
      },
      error: () => {
      }
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllInventoryAdjustmentData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllInventoryAdjustmentData();
    this.GetAllInventoryAdjustmentFilters();
  }

  onInventoryItemClicked(item: any) {
    this.ItemForm.patchValue({ inventoryItemId: item.inventoryItemId });
    this.ItemForm.get('inventoryItemId')?.markAsTouched();
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    if (!this.ItemForm.valid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    if (+this.ItemForm.value.quantityChange === 0) {
      this.toaster.warning('لا يمكن حفظ حركة بكمية تساوي صفر');
      return;
    }

    const payload = {
      ...this.ItemForm.value,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId,
      inventoryItemId: +this.ItemForm.value.inventoryItemId,
      quantityChange: +this.ItemForm.value.quantityChange
    };

    this.showLoader = true;
    if (payload.inventoryAdjustmentId === 0) {
      this.inventoryService.AddInventoryAdjustment(payload).subscribe(data => {
        this.HandleSaveResponse(data);
      });
      return;
    }

    this.inventoryService.UpdateInventoryAdjustment(payload).subscribe(data => {
      this.HandleSaveResponse(data);
    });
  }

  DeleteItem() {
    if (!this.InventoryAdjustmentId) {
      return;
    }

    this.showLoader = true;
    this.inventoryService.DeleteInventoryAdjustment(this.InventoryAdjustmentId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message || '');
        this.GetAllInventoryAdjustmentData();
        this.GetInventoryItems();
        this.GetAllInventoryAdjustmentFilters();
        this.modalService.dismissAll();
      } else {
        this.toaster.error(data.message || '');
      }
      this.showLoader = false;
    });
  }

  GetInventoryItemName(id: number): string {
    return this.InventoryItems.find(i => i.inventoryItemId === id)?.name || 'اختر عنصر المخزون';
  }

  private HandleSaveResponse(data: any) {
    if (data.isSuccess) {
      this.toaster.success(data.message || '');
      this.GetAllInventoryAdjustmentData();
      this.GetAllInventoryAdjustmentFilters();
      this.GetInventoryItems();
      this.modalService.dismissAll();
    } else {
      this.toaster.error(data.message || '');
    }

    this.showLoader = false;
  }
}
