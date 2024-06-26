import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { HomeSlideimageComponent } from './Components/home-slideimage/home-slideimage.component';
import { AdminActivityComponent } from './Components/admin-activity/admin-activity.component';
import { AdminEventComponent } from './Components/admin-event/admin-event.component';
import { AdminPhotoComponent } from './Components/admin-photo/admin-photo.component';
import { AdminUserComponent } from './Components/Settings/admin-user/admin-user.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AuthGuard } from '../Auth/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    data: { roles: ["SupperAdmin", "WebSite", "Services"] },
    children: [
      {
        path: 'home',
        component: AdminHomeComponent,
        canActivate: [AuthGuard],
        data: { roles: ["SupperAdmin", "WebSite", "Services"] },
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
