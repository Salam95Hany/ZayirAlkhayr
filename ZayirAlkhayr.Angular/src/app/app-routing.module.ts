import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './WebSite/home/home.component';
import { EventComponent } from './WebSite/event/event.component';
import { AboutusComponent } from './WebSite/aboutus/aboutus.component';
import { ActivityComponent } from './WebSite/activity/activity.component';
import { PhotosComponent } from './WebSite/photos/photos.component';
import { ActivityDetailsComponent } from './WebSite/activity-details/activity-details.component';
import { PhotoDetailsComponent } from './WebSite/photo-details/photo-details.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent, children: [
      { path: 'event', component: EventComponent },
      { path: 'aboutus', component: AboutusComponent },
      { path: 'activity', component: ActivityComponent },
      { path: 'activity-details', component: ActivityDetailsComponent },
      { path: 'photos', component: PhotosComponent },
      { path: 'photo-details', component: PhotoDetailsComponent },
      { path: '', redirectTo: 'activity', pathMatch: 'full' }
    ]
  },
  { path: '', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
