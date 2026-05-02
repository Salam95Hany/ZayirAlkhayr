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
import { FactoryResetComponent } from './Components/Setting/factory-reset/factory-reset.component';
import { UnitsComponent } from './Components/Inventory/units/units.component';
import { InventoryItemsComponent } from './Components/Inventory/inventory-items/inventory-items.component';
import { ItemRecipesComponent } from './Components/Inventory/item-recipes/item-recipes.component';
import { SuppliersComponent } from './Components/Inventory/suppliers/suppliers.component';
import { InventoryAdjustmentsComponent } from './Components/Inventory/inventory-adjustments/inventory-adjustments.component';
import { PurchasesComponent } from './Components/Inventory/purchases/purchases.component';
import { DailySalesReportComponent } from './Components/Reports/sales/daily-sales-report/daily-sales-report.component';
import { MonthlySalesReportComponent } from './Components/Reports/sales/monthly-sales-report/monthly-sales-report.component';
import { AllItemReportComponent } from './Components/Reports/items/all-item-report/all-item-report.component';
import { NeverSoldItemReportComponent } from './Components/Reports/items/never-sold-item-report/never-sold-item-report.component';
import { TopSellingItemReportComponent } from './Components/Reports/items/top-selling-item-report/top-selling-item-report.component';
import { LowestSellingItemReportComponent } from './Components/Reports/items/lowest-selling-item-report/lowest-selling-item-report.component';
import { FinalProfitsComponent } from './Components/Reports/final-profits/final-profits.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Admin'] },
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['لوحة التحكم'] },
      },
      {
        path: 'items',
        component: ItemsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['النظام', 'إدارة المطعم', 'العناصر'] },
      },
      {
        path: 'categories',
        component: CategoriesComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['النظام', 'إدارة المطعم', 'الفئات'] },
      },
      {
        path: 'order-list',
        component: OrderListComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['النظام', 'إدارة المطعم', 'قائمة الطلبات'] },
      },
      {
        path: 'customers',
        component: CustomersComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['النظام', 'إدارة المطعم', 'العملاء'] },
      },
      {
        path: 'inventory-units',
        component: UnitsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['المخزون', 'إدارة المخزون', 'الوحدات'] },
      },
      {
        path: 'inventory-items',
        component: InventoryItemsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['المخزون', 'إدارة المخزون', 'عناصر المخزون'] },
      },
      {
        path: 'inventory-item-recipes',
        component: ItemRecipesComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['المخزون', 'إدارة المخزون', 'وصفات الأصناف'] },
      },
      {
        path: 'inventory-suppliers',
        component: SuppliersComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['المخزون', 'إدارة المخزون', 'الموردون'] },
      },
      {
        path: 'inventory-adjustments',
        component: InventoryAdjustmentsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['المخزون', 'إدارة المخزون', 'حركة المخزون'] },
      },
      {
        path: 'inventory-purchases',
        component: PurchasesComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['المخزون', 'إدارة المخزون', 'المشتريات'] },
      },
      {
        path: 'daily-sales-reports',
        component: DailySalesReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['التقارير', 'تقارير المبيعات', 'المبيعات اليومية'] },
      },
      {
        path: 'monthly-sales-reports',
        component: MonthlySalesReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['التقارير', 'تقارير المبيعات', 'المبيعات الشهرية'] },
      },
      {
        path: 'all-item-reports',
        component: AllItemReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['التقارير', 'تقارير العناصر', 'كل العناصر'] },
      },
      {
        path: 'never-solditem-reports',
        component: NeverSoldItemReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['التقارير', 'تقارير العناصر', 'غير المباعة'] },
      },
      {
        path: 'top-sellingitem-reports',
        component: TopSellingItemReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['التقارير', 'تقارير العناصر', 'الأكثر مبيعًا'] },
      },
      {
        path: 'lowest-sellingitem-reports',
        component: LowestSellingItemReportComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['التقارير', 'تقارير العناصر', 'الأقل مبيعًا'] },
      },
      {
        path: 'final-profits',
        component: FinalProfitsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['التقارير', 'تقارير الأرباح', 'الأرباح النهائية'] },
      },
      {
        path: 'users',
        component: AdminUserComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['إعدادات النظام', 'الإعدادات', 'المستخدمون'] },
      },
      {
        path: 'backup',
        component: BackupComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['إعدادات النظام', 'الإعدادات', 'النسخ الاحتياطية'] },
      },
      {
        path: 'factory-reset',
        component: FactoryResetComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'], breadcrumb: ['إعدادات النظام', 'الإعدادات', 'إعادة ضبط المصنع'] },
      },
      {
        path: 'user-profile',
        component: UserProfileComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin', 'Cashier'], breadcrumb: ['إعدادات النظام', 'الملف الشخصي'] },
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
