import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AdminUserComponent } from './Components/Setting/admin-user/admin-user.component';
import { BackupComponent } from './Components/Setting/backup/backup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RoleCheckerDirective } from './Directives/role-checker.directive';
import { SearchArryPipe } from './Pipes/search-arry.pipe';
import { ngxLoadingAnimationTypes, NgxLoadingModule } from "ngx-loading";
import { AdminHeaderComponent } from './Shared/admin-header/admin-header.component';
import { AdminSideMenuComponent } from './Shared/admin-side-menu/admin-side-menu.component';
import { AdminFiltersComponent } from './Shared/admin-filters/admin-filters.component';
import { AdminPaginationComponent } from './Shared/admin-pagination/admin-pagination.component';
import { CustomersComponent } from './Components/Customer/customers/customers.component';
import { ItemsComponent } from './Components/POS/items/items.component';
import { CategoriesComponent } from './Components/POS/categories/categories.component';
import { OrderListComponent } from './Components/POS/order-list/order-list.component';
import { DashboardComponent } from './Shared/dashboard/dashboard.component';
import { EnumTextPipe } from './Pipes/enum-text.pipe';
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { TimeAgoTodayPipe } from './Pipes/time-ago-today.pipe';
import { SharedModule } from './Shared/shared.module';
import { UserProfileComponent } from './Components/Setting/user-profile/user-profile.component';
import { FactoryResetComponent } from './Components/Setting/factory-reset/factory-reset.component';
import { SalesReportsComponent } from './Components/Reports/sales/sales-reports/sales-reports.component';
import { ItemsReportComponent } from './Components/Reports/sales/items-report/items-report.component';
import { OrdertypeReportComponent } from './Components/Reports/sales/ordertype-report/ordertype-report.component';
import { CustomerReportComponent } from './Components/Reports/sales/customer-report/customer-report.component';
import { SalesbytimeReportComponent } from './Components/Reports/sales/salesbytime-report/salesbytime-report.component';
import { InventoryItemsComponent } from './Components/Inventory/inventory-items/inventory-items.component';
import { SuppliersComponent } from './Components/Inventory/suppliers/suppliers.component';
import { InventoryAdjustmentsComponent } from './Components/Inventory/inventory-adjustments/inventory-adjustments.component';
import { PurchasesComponent } from './Components/Inventory/purchases/purchases.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminHeaderComponent,
    AdminSideMenuComponent,
    AdminFiltersComponent,
    AdminPaginationComponent,
    AdminUserComponent,
    BackupComponent,
    RoleCheckerDirective,
    SearchArryPipe,
    CustomersComponent,
    ItemsComponent,
    CategoriesComponent,
    OrderListComponent,
    DashboardComponent,
    EnumTextPipe,
    TimeAgoTodayPipe,
    UserProfileComponent,
    FactoryResetComponent,
    SalesReportsComponent,
    ItemsReportComponent,
    OrdertypeReportComponent,
    CustomerReportComponent,
    SalesbytimeReportComponent,
    InventoryItemsComponent,
    SuppliersComponent,
    InventoryAdjustmentsComponent,
    PurchasesComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    PaginationModule.forRoot(),
    NgxDaterangepickerMd.forRoot(),
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.circleSwish,
      backdropBackgroundColour: 'rgba(0, 18, 59, 0.6)',
      backdropBorderRadius: '3px',
      primaryColour: '#74c173',
      fullScreenBackdrop: true
    }),
    AdminRoutingModule
  ],
  providers: [DatePipe]
})
export class AdminModule { }
