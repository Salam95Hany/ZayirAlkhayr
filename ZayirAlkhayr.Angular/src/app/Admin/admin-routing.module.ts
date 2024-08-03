import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { HomeSlideimageComponent } from './Components/WebSite/home-slideimage/home-slideimage.component';
import { AdminActivityComponent } from './Components/WebSite/admin-activity/admin-activity.component';
import { AdminEventComponent } from './Components/WebSite/admin-event/admin-event.component';
import { AdminPhotoComponent } from './Components/WebSite/admin-photo/admin-photo.component';
import { AdminUserComponent } from './Components/Settings/admin-user/admin-user.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AuthGuard } from '../Auth/auth.guard';
import { BeneFactorComponent } from './Components/BeneFactors/bene-factor/bene-factor.component';
import { BeneFactorDetailsComponent } from './Components/BeneFactors/bene-factor-details/bene-factor-details.component';
import { BeneFactorTypesComponent } from './Components/BeneFactors/bene-factor-types/bene-factor-types.component';
import { BeneFactorNotesComponent } from './Components/BeneFactors/bene-factor-notes/bene-factor-notes.component';
import { AccountImportMonyComponent } from './Components/BeneFactors/account-import-mony/account-import-mony.component';
import { AccountExportMonyComponent } from './Components/BeneFactors/account-export-mony/account-export-mony.component';
import { BeneFactorNationalitiesComponent } from './Components/BeneFactors/bene-factor-nationalities/bene-factor-nationalities.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    data: { roles: ["SupperAdmin", "WebSite", "Services", "BeneFactors", "Accounts"] },
    children: [
      {
        path: 'home',
        component: AdminHomeComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite", "Services", "BeneFactors", "Accounts"] },
      },
      {
        path: 'home-slideimage',
        component: HomeSlideimageComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite"] }
      },
      {
        path: 'activity',
        component: AdminActivityComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite"] }
      },
      {
        path: 'event',
        component: AdminEventComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite"] }
      },
      {
        path: 'photo',
        component: AdminPhotoComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite"] }
      },
      {
        path: 'benefactors',
        component: BeneFactorComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors"] }
      },
      {
        path: 'benefactor-detail',
        component: BeneFactorDetailsComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors"] }
      },
      {
        path: 'benefactor-type',
        component: BeneFactorTypesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors"] }
      },
      {
        path: 'benefactor-note',
        component: BeneFactorNotesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors"] }
      },
      {
        path: 'benefactor-nationality',
        component: BeneFactorNationalitiesComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "BeneFactors"] }
      },
      {
        path: 'account-import-money',
        component: AccountImportMonyComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Accounts"] }
      },
      {
        path: 'account-export-money',
        component: AccountExportMonyComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "Accounts"] }
      },
      {
        path: 'user',
        component: AdminUserComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin"] }
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
