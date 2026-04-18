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
  RoleName = '';
  UserModel: any;
  Roles = [
    { nameEn: 'Admin', nameAr: 'مدير' },
    { nameEn: 'Cashier', nameAr: 'كاشير' }
  ];
  POS = ['categories', 'items', 'order-list', 'customers'];
  Inventory = ['inventory-items', 'inventory-suppliers', 'inventory-adjustments', 'inventory-purchases'];
  Settings = ['users', 'backup','factory-reset'];
  SalesReports = ['daily-sales-reports', 'monthly-sales-reports'];
  ItemsReports = ['all-item-reports', 'top-sellingitem-reports', 'lowest-sellingitem-reports', 'never-solditem-reports'];
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
  }

  onCloseSidemenuFromOverlay() {
    this.closeSideMenuFromOverlayEvent.emit();
  }
}
