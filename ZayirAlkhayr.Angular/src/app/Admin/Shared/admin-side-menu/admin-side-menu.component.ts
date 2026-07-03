import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-side-menu',
  templateUrl: './admin-side-menu.component.html',
  styleUrls: ['./admin-side-menu.component.css']
})
export class AdminSideMenuComponent implements OnInit {
  @Input() isCollapsing = false;
  @Output() closeSideMenuFromOverlayEvent = new EventEmitter<boolean>();
  isCollapsed_1 = true;
  isCollapsed_2 = true;
  isCollapsed_3 = true;
  isCollapsed_4 = true;
  isCollapsed_5 = true;
  isCollapsed_6 = true;
  isCollapsed_7 = true;
  isCollapsed_8 = true;
  RoleName = '';
  UserModel: any;
  Roles = [
    { nameEn: 'Admin', nameAr: 'مدير' },
    { nameEn: 'Cashier', nameAr: 'كاشير' }
  ];
  POS = ['categories', 'items', 'order-list', 'customers','tables'];
  Inventory = ['inventory-units', 'inventory-items', 'inventory-item-recipes', 'inventory-adjustments'];
  Settings = ['users', 'backup', 'factory-reset'];
  SalesReports = ['daily-sales-reports', 'monthly-sales-reports'];
  ItemsReports = ['all-item-reports', 'top-sellingitem-reports', 'lowest-sellingitem-reports', 'never-solditem-reports'];
  Purchase = ['inventory-suppliers', 'inventory-purchases'];
  Emplyees = ['jop-title', 'employees','employee-salary'];
  Expenses = ['expense-types', 'expenses'];
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.RoleName = this.Roles.find(i => i.nameEn == this.UserModel?.role)?.nameAr;
    let url = this.router.url.split('/')[2];
    if (this.POS.includes(url))
      this.isCollapsed_1 = false;
    else if (this.Inventory.includes(url))
      this.isCollapsed_4 = false;
    else if (this.Settings.includes(url))
      this.isCollapsed_2 = false;
    else if (this.SalesReports.includes(url))
      this.isCollapsed_3 = false;
    else if (this.ItemsReports.includes(url))
      this.isCollapsed_5 = false;
    else if (this.Purchase.includes(url))
      this.isCollapsed_6 = false;
    else if (this.Emplyees.includes(url))
      this.isCollapsed_7 = false;
    else if (this.Expenses.includes(url))
      this.isCollapsed_8 = false;
  }

  onCloseSidemenuFromOverlay() {
    this.closeSideMenuFromOverlayEvent.emit();
  }
}
