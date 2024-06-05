import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AdminHeaderComponent } from './admin-header/admin-header.component';
import { AdminSideMenuComponent } from './admin-side-menu/admin-side-menu.component';
import { HomeSlideimageComponent } from './Components/home-slideimage/home-slideimage.component';
import { AdminActivityComponent } from './Components/admin-activity/admin-activity.component';
import { AdminActivitydetailsComponent } from './Components/admin-activitydetails/admin-activitydetails.component';
import { AdminEventComponent } from './Components/admin-event/admin-event.component';
import { AdminPhotoComponent } from './Components/admin-photo/admin-photo.component';
import { AdminPhotodetailsComponent } from './Components/admin-photodetails/admin-photodetails.component';
import { AdminFiltersComponent } from './admin-filters/admin-filters.component';
import { AdminPaginationComponent } from './admin-pagination/admin-pagination.component';
import { AdminUserComponent } from './Components/Settings/admin-user/admin-user.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AdminComponent,
    AdminHeaderComponent,
    AdminSideMenuComponent,
    HomeSlideimageComponent,
    AdminActivityComponent,
    AdminActivitydetailsComponent,
    AdminEventComponent,
    AdminPhotoComponent,
    AdminPhotodetailsComponent,
    AdminFiltersComponent,
    AdminPaginationComponent,
    AdminUserComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    PaginationModule.forRoot(),
    AdminRoutingModule
  ]
})
export class AdminModule { }
