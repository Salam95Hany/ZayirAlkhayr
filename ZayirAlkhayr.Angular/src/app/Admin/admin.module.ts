import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { GeneralStatusComponent } from './SocialServices/general-status/general-status.component';
import { AdminHeaderComponent } from './admin-header/admin-header.component';
import { AdminSideMenuComponent } from './admin-side-menu/admin-side-menu.component';


@NgModule({
  declarations: [
    AdminComponent,
    GeneralStatusComponent,
    AdminHeaderComponent,
    AdminSideMenuComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
