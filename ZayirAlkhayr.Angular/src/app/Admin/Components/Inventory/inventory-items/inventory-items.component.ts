import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-inventory-items',
  templateUrl: './inventory-items.component.html',
  styleUrls: ['./inventory-items.component.css']
})
export class InventoryItemsComponent {
  UserModel: any;
  isFilter = false;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  LowStockCount = 0;
  InventoryItemId: number | null = null;
  Results: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryName: 'SearchText',
      categoryDisplayName: 'عنصر المخزون',
      filterType: 'SearchText',
      itemId: ''
    },
    {
      categoryName: 'LowStockOnly',
      categoryDisplayName: 'حالة المخزون',
      filterType: 'Checkbox',
      filterItems: [
        {
          itemId: '1',
          itemKey: 'تحت الحد الأدنى',
          itemValue: '',
          isChecked: false
        }
      ]
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
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.FormInit();
    this.GetAllInventoryItems();
    this.GetLowStockInventoryItemsCount();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      inventoryItemId: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      unitId: [null, [Validators.required]],
      currentQuantity: [0, [Validators.required]],
      minQuantity: [0, [Validators.required]],
      insertUser: null,
      updateUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.patchValue({
      inventoryItemId: 0,
      currentQuantity: 0,
      minQuantity: 0,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      inventoryItemId: item.inventoryItemId,
      name: item.name,
      unitId: item.unitId,
      currentQuantity: item.currentQuantity,
      minQuantity: item.minQuantity,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
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
    this.InventoryItemId = item.inventoryItemId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllInventoryItems() {
    this.showLoader = true;
    this.inventoryService.GetAllInventoryItems(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.Total = data.totalCount || 0;
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل عناصر المخزون');
      }
    });
  }

  GetLowStockInventoryItemsCount() {
    this.inventoryService.GetLowStockInventoryItems().subscribe({
      next: (data) => this.LowStockCount = data.results?.length || 0,
      error: () => this.LowStockCount = 0
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllInventoryItems();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllInventoryItems();
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
      updateUser: this.UserModel?.userId,
      unitId: +this.ItemForm.value.unitId,
      currentQuantity: +this.ItemForm.value.currentQuantity,
      minQuantity: +this.ItemForm.value.minQuantity
    };

    this.showLoader = true;
    if (payload.inventoryItemId === 0) {
      this.inventoryService.AddNewInventoryItem(payload).subscribe(data => {
        this.HandleSaveResponse(data);
      });
      return;
    }

    this.inventoryService.UpdateInventoryItem(payload).subscribe(data => {
      this.HandleSaveResponse(data);
    });
  }

  DeleteItem() {
    if (!this.InventoryItemId) {
      return;
    }

    this.showLoader = true;
    this.inventoryService.DeleteInventoryItem(this.InventoryItemId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message || '');
        this.GetAllInventoryItems();
        this.GetLowStockInventoryItemsCount();
        this.modalService.dismissAll();
      } else {
        this.toaster.error(data.message || '');
      }
      this.showLoader = false;
    });
  }

  IsLowStock(item: any): boolean {
    return (+item?.currentQuantity || 0) <= (+item?.minQuantity || 0);
  }

  private HandleSaveResponse(data: any) {
    if (data.isSuccess) {
      this.toaster.success(data.message || '');
      this.GetAllInventoryItems();
      this.GetLowStockInventoryItemsCount();
      this.modalService.dismissAll();
    } else {
      this.toaster.error(data.message || '');
    }

    this.showLoader = false;
  }
}
