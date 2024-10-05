import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { HomeSlideimageComponent } from './Components/WebSite/home-slideimage/home-slideimage.component';
import { AdminActivityComponent } from './Components/WebSite/admin-activity/admin-activity.component';
import { AdminEventComponent } from './Components/WebSite/admin-event/admin-event.component';
import { AdminPhotoComponent } from './Components/WebSite/admin-photo/admin-photo.component';
import { AdminUserComponent } from './Components/Settings/admin-user/admin-user.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RoleCheckerDirective } from './Directives/role-checker.directive';
import { SearchArryPipe } from './Pipes/search-arry.pipe';
import { ngxLoadingAnimationTypes, NgxLoadingModule } from "ngx-loading";
import { BeneFactorComponent } from './Components/BeneFactors/bene-factor/bene-factor.component';
import { BeneFactorDetailsComponent } from './Components/BeneFactors/bene-factor-details/bene-factor-details.component';
import { BeneFactorTypesComponent } from './Components/BeneFactors/bene-factor-types/bene-factor-types.component';
import { BeneFactorNotesComponent } from './Components/BeneFactors/bene-factor-notes/bene-factor-notes.component';
import { AccountImportMonyComponent } from './Components/Tasks/account-import-mony/account-import-mony.component';
import { AccountExportMonyComponent } from './Components/Tasks/account-export-mony/account-export-mony.component';
import { BeneFactorNationalitiesComponent } from './Components/BeneFactors/bene-factor-nationalities/bene-factor-nationalities.component';
import { AdminHeaderComponent } from './Shared/admin-header/admin-header.component';
import { AdminSideMenuComponent } from './Shared/admin-side-menu/admin-side-menu.component';
import { AdminFiltersComponent } from './Shared/admin-filters/admin-filters.component';
import { AdminPaginationComponent } from './Shared/admin-pagination/admin-pagination.component';
import { AdminHomeComponent } from './Shared/admin-home/admin-home.component';
import { AdminBackupComponent } from './Components/Settings/admin-backup/admin-backup.component';
import { GeneralTasksComponent } from './Components/Tasks/general-tasks/general-tasks.component';
import { DailyTasksComponent } from './Components/Tasks/daily-tasks/daily-tasks.component';
import { FamilyStatusComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/family-status/family-status.component';
import { FamilyDataComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/family-data/family-data.component';
import { FamilyIncomeDataComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/family-income-data/family-income-data.component';
import { FamilyExpensesDataComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/family-expenses-data/family-expenses-data.component';
import { FamilyMedicalComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/family-medical/family-medical.component';
import { ReviewersComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/reviewers/reviewers.component';
import { AddFamilyStatusComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/add-family-status.component';
import { FamilyNeedComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/family-need/family-need.component';
import { FamilyNationalityComponent } from './Components/GeneralServices/generalStatus/family-nationality/family-nationality.component';
import { FamilyNeedsComponent } from './Components/GeneralServices/generalStatus/family-needs/family-needs.component';
import { FamilyCategoriesComponent } from './Components/GeneralServices/generalStatus/family-categories/family-categories.component';
import { FamilyPatienttypesComponent } from './Components/GeneralServices/generalStatus/family-patienttypes/family-patienttypes.component';
import { AllFamilyStatusComponent } from './Components/GeneralServices/generalStatus/all-family-status/all-family-status.component';

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
    BeneFactorNationalitiesComponent,
    AdminBackupComponent,
    GeneralTasksComponent,
    DailyTasksComponent,
    FamilyStatusComponent,
    FamilyDataComponent,
    FamilyIncomeDataComponent,
    FamilyExpensesDataComponent,
    FamilyMedicalComponent,
    ReviewersComponent,
    AddFamilyStatusComponent,
    FamilyNeedComponent,
    FamilyNationalityComponent,
    FamilyNeedsComponent,
    FamilyCategoriesComponent,
    FamilyPatienttypesComponent,
    AllFamilyStatusComponent
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
