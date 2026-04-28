import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-units',
  templateUrl: './units.component.html',
  styleUrls: ['./units.component.css']
})
export class UnitsComponent {
  UserModel: any;
  isFilter = false;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  UnitsInUseCount = 0;
  UnitId: number | null = null;
  Results: any[] = [];
  InventoryItems: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryName: 'SearchText',
      categoryDisplayName: 'الوحدة',
      filterType: 'SearchText',
      itemId: ''
    }
  ];
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
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.FormInit();
    this.GetAllUnits();
    this.GetInventoryItems();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      unitId: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null,
      updateUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.patchValue({
      unitId: 0,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      unitId: item.unitId,
      name: item.name,
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
    this.UnitId = item.unitId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllUnits() {
    this.showLoader = true;
    this.inventoryService.GetAllUnits(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.Total = data.totalCount || 0;
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل الوحدات');
      }
    });
  }

  GetInventoryItems() {
    this.inventoryService.GetAllInventoryItems(this.InventoryItemsPagingFilter).subscribe({
      next: (data) => {
        this.InventoryItems = data.results || [];
        this.UpdateUnitsInUseCount();
      },
      error: () => {
        this.InventoryItems = [];
        this.UpdateUnitsInUseCount();
      }
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllUnits();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllUnits();
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

    this.showLoader = true;
    if (payload.unitId === 0) {
      this.inventoryService.AddNewUnit(payload).subscribe(data => {
        this.HandleSaveResponse(data);
      });
      return;
    }

    this.inventoryService.UpdateUnit(payload).subscribe(data => {
      this.HandleSaveResponse(data);
    });
  }

  DeleteItem() {
    if (!this.UnitId) {
      return;
    }

    this.showLoader = true;
    this.inventoryService.DeleteUnit(this.UnitId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message || '');
        this.GetAllUnits();
        this.GetInventoryItems();
        this.modalService.dismissAll();
      } else {
        this.toaster.error(data.message || '');
      }
      this.showLoader = false;
    });
  }

  GetLinkedItemsCount(unitId: number): number {
    return this.InventoryItems.filter(item => item.unitId === unitId).length;
  }

  private UpdateUnitsInUseCount() {
    this.UnitsInUseCount = new Set(
      this.InventoryItems
        .map(item => item.unitId)
        .filter((unitId: number | null | undefined) => !!unitId)
    ).size;
  }

  private HandleSaveResponse(data: any) {
    if (data.isSuccess) {
      this.toaster.success(data.message || '');
      this.GetAllUnits();
      this.GetInventoryItems();
      this.modalService.dismissAll();
    } else {
      this.toaster.error(data.message || '');
    }

    this.showLoader = false;
  }
}
