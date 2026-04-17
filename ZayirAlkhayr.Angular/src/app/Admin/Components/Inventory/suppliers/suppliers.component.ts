import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-suppliers',
  templateUrl: './suppliers.component.html',
  styleUrls: ['./suppliers.component.css']
})
export class SuppliersComponent {
  UserModel: any;
  isFilter = false;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  ContactableSuppliers = 0;
  SupplierId: number | null = null;
  Results: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryName: 'SearchText',
      categoryDisplayName: 'المورد',
      filterType: 'SearchText',
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
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.FormInit();
    this.GetAllSuppliers();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      supplierId: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      phone: ['', [Validators.required, this.formService.noSpaceValidator]],
      address: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null,
      updateUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.patchValue({
      supplierId: 0,
      insertUser: this.UserModel?.userId,
      updateUser: this.UserModel?.userId
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      supplierId: item.supplierId,
      name: item.name,
      phone: item.phone,
      address: item.address,
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
    this.SupplierId = item.supplierId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllSuppliers() {
    this.showLoader = true;
    this.inventoryService.GetAllSuppliers(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.Total = data.totalCount || 0;
        this.ContactableSuppliers = this.Results.filter(i => !!i.phone).length;
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
    this.GetAllSuppliers();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllSuppliers();
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
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
    if (payload.supplierId === 0) {
      this.inventoryService.AddNewSupplier(payload).subscribe(data => {
        this.HandleSaveResponse(data);
      });
      return;
    }

    this.inventoryService.UpdateSupplier(payload).subscribe(data => {
      this.HandleSaveResponse(data);
    });
  }

  DeleteItem() {
    if (!this.SupplierId) {
      return;
    }

    this.showLoader = true;
    this.inventoryService.DeleteSupplier(this.SupplierId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message || '');
        this.GetAllSuppliers();
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
      this.GetAllSuppliers();
      this.modalService.dismissAll();
    } else {
      this.toaster.error(data.message || '');
    }

    this.showLoader = false;
  }
}
