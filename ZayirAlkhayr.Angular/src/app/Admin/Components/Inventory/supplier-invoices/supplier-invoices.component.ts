import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { InventoryService } from 'src/app/Admin/Services/inventory.service';

@Component({
  selector: 'app-supplier-invoices',
  templateUrl: './supplier-invoices.component.html',
  styleUrls: ['./supplier-invoices.component.css']
})
export class SupplierInvoicesComponent implements OnInit {
  UserModel: any;
  isFilter = true;
  showLoader = false;
  Total = 0;
  SupplierId: any;
  SupplierName = '';
  Results: any[] = [];
  FilterList: FilterModel[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
  };

  constructor(
    private inventoryService: InventoryService,
    private toaster: ToastrService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.SupplierId = this.route.snapshot.queryParamMap.get('supplierId');
    this.PagingFilter.supplierId = this.SupplierId;
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || 'null');
    this.GetAllSupplierInvicesData();
    this.GetAllSupplierInvicesFilters();
  }

  GetAllSupplierInvicesData() {
    this.showLoader = true;
    this.inventoryService.GetAllSupplierInvicesData(this.PagingFilter).subscribe({
      next: (data) => {
        this.Results = data.results || [];
        this.SupplierName = this.Results[0]?.supplierName;
        this.Total = data.totalCount || 0;
        this.showLoader = false;
      },
      error: () => {
        this.showLoader = false;
        this.toaster.error('تعذر تحميل الموردين');
      }
    });
  }

  GetAllSupplierInvicesFilters() {
    this.inventoryService.GetAllSupplierInvicesFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data.results;
    })
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllSupplierInvicesData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.currentpage = 1;
    this.PagingFilter.filterList = filterList;
    this.GetAllSupplierInvicesData();
    this.GetAllSupplierInvicesFilters();
  }
}
