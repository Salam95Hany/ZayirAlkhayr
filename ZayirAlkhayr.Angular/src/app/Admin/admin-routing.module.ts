import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { HomeSlideimageComponent } from './Components/WebSite/home-slideimage/home-slideimage.component';
import { AdminActivityComponent } from './Components/WebSite/admin-activity/admin-activity.component';
import { AdminEventComponent } from './Components/WebSite/admin-event/admin-event.component';
import { AdminPhotoComponent } from './Components/WebSite/admin-photo/admin-photo.component';
import { AdminUserComponent } from './Components/Settings/admin-user/admin-user.component';
import { AuthGuard } from '../Auth/auth.guard';
import { BeneFactorComponent } from './Components/BeneFactors/bene-factor/bene-factor.component';
import { BeneFactorDetailsComponent } from './Components/BeneFactors/bene-factor-details/bene-factor-details.component';
import { BeneFactorTypesComponent } from './Components/BeneFactors/bene-factor-types/bene-factor-types.component';
import { BeneFactorNotesComponent } from './Components/BeneFactors/bene-factor-notes/bene-factor-notes.component';
import { AccountImportMonyComponent } from './Components/Tasks/account-import-mony/account-import-mony.component';
import { AccountExportMonyComponent } from './Components/Tasks/account-export-mony/account-export-mony.component';
import { BeneFactorNationalitiesComponent } from './Components/BeneFactors/bene-factor-nationalities/bene-factor-nationalities.component';
import { AdminHomeComponent } from './Shared/admin-home/admin-home.component';
import { AdminBackupComponent } from './Components/Settings/admin-backup/admin-backup.component';
import { GeneralTasksComponent } from './Components/Tasks/general-tasks/general-tasks.component';
import { DailyTasksComponent } from './Components/Tasks/daily-tasks/daily-tasks.component';
import { AddFamilyStatusComponent } from './Components/GeneralServices/generalStatus/addFamilyStatus/add-family-status.component';
import { FamilyNationalityComponent } from './Components/GeneralServices/generalStatus/family-nationality/family-nationality.component';
import { FamilyNeedsComponent } from './Components/GeneralServices/generalStatus/family-needs/family-needs.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    data: { roles: ["SupperAdmin", "WebSite", "Services", "BeneFactors", "Accounts", "Admin"] },
    children: [
      {
        path: 'home',
        component: AdminHomeComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite", "Services", "BeneFactors", "Accounts", "Admin"] },
      },
      {
        path: 'home-slideimage',
        component: HomeSlideimageComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite", "Admin"] }
      },
      {
        path: 'activity',
        component: AdminActivityComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite", "Admin"] }
      },
      {
        path: 'event',
        component: AdminEventComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite", "Admin"] }
      },
      {
        path: 'photo',
        component: AdminPhotoComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite", "Admin"] }
      },
      {
        path: 'benefactors',
        component: BeneFactorComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors", "Admin"] }
      },
      {
        path: 'benefactor-detail',
        component: BeneFactorDetailsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors", "Admin"] }
      },
      {
        path: 'benefactor-type',
        component: BeneFactorTypesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors", "Admin"] }
      },
      {
        path: 'benefactor-note',
        component: BeneFactorNotesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors", "Admin"] }
      },
      {
        path: 'benefactor-nationality',
        component: BeneFactorNationalitiesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors", "Admin"] }
      },
      {
        path: 'account-import-money',
        component: AccountImportMonyComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Accounts", "Admin"] }
      },
      {
        path: 'account-export-money',
        component: AccountExportMonyComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Accounts", "Admin"] }
      },
      {
        path: 'general-tasks',
        component: GeneralTasksComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Admin"] }
      },
      {
        path: 'daily-tasks',
        component: DailyTasksComponent,
      },
      {
        path: 'user',
        component: AdminUserComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin"] }
      },
      {
        path: 'backup',
        component: AdminBackupComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin"] }
      },
      {
        path: 'add-family-status',
        component: AddFamilyStatusComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Admin", "Services"] }
      },
      {
        path: 'family-nationality',
        component: FamilyNationalityComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Admin", "Services"] }
      },
      {
        path: 'family-needs',
        component: FamilyNeedsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Admin", "Services"] }
      },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
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
