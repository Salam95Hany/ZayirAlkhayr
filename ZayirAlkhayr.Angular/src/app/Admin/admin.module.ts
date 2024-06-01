import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { AdminHeaderComponent } from './admin-header/admin-header.component';
import { AdminSideMenuComponent } from './admin-side-menu/admin-side-menu.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    AdminComponent,
    AdminHeaderComponent,
    AdminSideMenuComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    AdminRoutingModule
  ],
  exports: [AdminHeaderComponent, AdminSideMenuComponent]
})
export class AdminModule { }
