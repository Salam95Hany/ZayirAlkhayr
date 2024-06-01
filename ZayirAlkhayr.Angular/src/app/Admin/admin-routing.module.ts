import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { HomeSlideimageComponent } from './Components/home-slideimage/home-slideimage.component';
import { AdminActivityComponent } from './Components/admin-activity/admin-activity.component';
import { AdminActivitydetailsComponent } from './Components/admin-activitydetails/admin-activitydetails.component';
import { AdminEventComponent } from './Components/admin-event/admin-event.component';
import { AdminPhotoComponent } from './Components/admin-photo/admin-photo.component';
import { AdminPhotodetailsComponent } from './Components/admin-photodetails/admin-photodetails.component';

const routes: Routes = [
  {
    path: '', component: AdminComponent, children: [
      { path: 'home-slideimage', component: HomeSlideimageComponent },
      { path: 'activity', component: AdminActivityComponent },
      { path: 'activity-details', component: AdminActivitydetailsComponent },
      { path: 'event', component: AdminEventComponent },
      { path: 'photo', component: AdminPhotoComponent },
      { path: 'photo-details', component: AdminPhotodetailsComponent },
      { path: '', redirectTo: 'home-slideimage', pathMatch: 'full' },
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
