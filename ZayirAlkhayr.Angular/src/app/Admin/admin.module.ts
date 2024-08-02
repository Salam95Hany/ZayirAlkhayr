import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AdminHeaderComponent } from './admin-header/admin-header.component';
import { AdminSideMenuComponent } from './admin-side-menu/admin-side-menu.component';
import { HomeSlideimageComponent } from './Components/home-slideimage/home-slideimage.component';
import { AdminActivityComponent } from './Components/admin-activity/admin-activity.component';
import { AdminEventComponent } from './Components/admin-event/admin-event.component';
import { AdminPhotoComponent } from './Components/admin-photo/admin-photo.component';
import { AdminFiltersComponent } from './admin-filters/admin-filters.component';
import { AdminPaginationComponent } from './admin-pagination/admin-pagination.component';
import { AdminUserComponent } from './Components/Settings/admin-user/admin-user.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RoleCheckerDirective } from './Directives/role-checker.directive';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { SearchArryPipe } from './Pipes/search-arry.pipe';
import { ngxLoadingAnimationTypes, NgxLoadingModule } from "ngx-loading";
import { BeneFactorComponent } from './Components/bene-factor/bene-factor.component';
import { BeneFactorDetailsComponent } from './Components/bene-factor-details/bene-factor-details.component';
import { BeneFactorTypesComponent } from './Components/bene-factor-types/bene-factor-types.component';
import { BeneFactorNotesComponent } from './Components/bene-factor-notes/bene-factor-notes.component';
import { AccountImportMonyComponent } from './Components/account-import-mony/account-import-mony.component';
import { AccountExportMonyComponent } from './Components/account-export-mony/account-export-mony.component';
import { BeneFactorNationalitiesComponent } from './Components/bene-factor-nationalities/bene-factor-nationalities.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminHeaderComponent,
    AdminSideMenuComponent,
    HomeSlideimageComponent,
    AdminActivityComponent,
    AdminEventComponent,
    AdminPhotoComponent,
    AdminFiltersComponent,
    AdminPaginationComponent,
    AdminUserComponent,
    RoleCheckerDirective,
    AdminHomeComponent,
    SearchArryPipe,
    BeneFactorComponent,
    BeneFactorDetailsComponent,
    BeneFactorTypesComponent,
    BeneFactorNotesComponent,
    AccountImportMonyComponent,
    AccountExportMonyComponent,
    BeneFactorNationalitiesComponent
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
