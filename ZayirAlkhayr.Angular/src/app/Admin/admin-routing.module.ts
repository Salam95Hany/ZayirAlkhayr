import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AuthGuard } from '../Auth/auth.guard';
import { DashboardComponent } from './Shared/dashboard/dashboard.component';
import { ItemsComponent } from './Components/POS/items/items.component';
import { CategoriesComponent } from './Components/POS/categories/categories.component';
import { OrderListComponent } from './Components/POS/order-list/order-list.component';
import { CustomersComponent } from './Components/Customer/customers/customers.component';
import { AdminUserComponent } from './Components/Setting/admin-user/admin-user.component';
import { BackupComponent } from './Components/Setting/backup/backup.component';
import { UserProfileComponent } from './Components/Setting/user-profile/user-profile.component';
import { SalesReportsComponent } from './Components/Reports/sales/sales-reports/sales-reports.component';
import { ItemsReportComponent } from './Components/Reports/sales/items-report/items-report.component';
import { OrdertypeReportComponent } from './Components/Reports/sales/ordertype-report/ordertype-report.component';
import { CustomerReportComponent } from './Components/Reports/sales/customer-report/customer-report.component';
import { SalesbytimeReportComponent } from './Components/Reports/sales/salesbytime-report/salesbytime-report.component';
import { InventoryItemsComponent } from './Components/Inventory/inventory-items/inventory-items.component';
import { SuppliersComponent } from './Components/Inventory/suppliers/suppliers.component';
import { InventoryAdjustmentsComponent } from './Components/Inventory/inventory-adjustments/inventory-adjustments.component';
import { PurchasesComponent } from './Components/Inventory/purchases/purchases.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    data: { roles: ["Admin", "Cashier"] },
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
      },
      {
        path: 'items',
        component: ItemsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
      },
      {
        path: 'categories',
        component: CategoriesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
      },
      {
        path: 'order-list',
        component: OrderListComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
      },
      {
        path: 'customers',
        component: CustomersComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
      },
      {
        path: 'inventory-items',
        component: InventoryItemsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'inventory-suppliers',
        component: SuppliersComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'inventory-adjustments',
        component: InventoryAdjustmentsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'inventory-purchases',
        component: PurchasesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'sales-reports',
        component: SalesReportsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'items-reports',
        component: ItemsReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'ordertype-reports',
        component: OrdertypeReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'customers-reports',
        component: CustomerReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'salesbytime-reports',
        component: SalesbytimeReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'users',
        component: AdminUserComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'backup',
        component: BackupComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin"] },
      },
      {
        path: 'user-profile',
        component: UserProfileComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    ]
  },
  { path: '', redirectTo: '', pathMatch: 'full' },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
