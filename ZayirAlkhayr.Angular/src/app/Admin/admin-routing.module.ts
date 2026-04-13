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
import { UserProfileComponent } from './Components/Setting/user-profile/user-profile.component';
import { SalesReportsComponent } from './Components/Reports/sales-reports/sales-reports.component';

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
        path: 'sales-reports',
        component: SalesReportsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
      },
      {
        path: 'users',
        component: AdminUserComponent,
        canActivate: [AuthGuard],
        data: { roles: ["Admin", "Cashier"] },
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
