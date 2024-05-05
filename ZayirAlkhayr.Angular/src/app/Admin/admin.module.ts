import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { GeneralStatusComponent } from './SocialServices/general-status/general-status.component';


@NgModule({
  declarations: [
    AdminComponent,
    GeneralStatusComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
