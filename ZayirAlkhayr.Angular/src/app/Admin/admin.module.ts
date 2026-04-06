import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AdminUserComponent } from './Components/Setting/admin-user/admin-user.component';
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

@NgModule({
  declarations: [
    AdminComponent,
    AdminHeaderComponent,
    AdminSideMenuComponent,
    AdminFiltersComponent,
    AdminPaginationComponent,
    AdminUserComponent,
    RoleCheckerDirective,
    SearchArryPipe,
    CustomersComponent,
    ItemsComponent,
    CategoriesComponent,
    OrderListComponent,
    DashboardComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.circleSwish,
      backdropBackgroundColour: 'rgba(0, 18, 59, 0.6)',
      backdropBorderRadius: '3px',
      primaryColour: '#74c173',
      fullScreenBackdrop: true
    }),
    AdminRoutingModule
  ],
  providers:[DatePipe]
})
export class AdminModule { }
