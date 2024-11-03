import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './WebSite/home/home.component';
import { EventComponent } from './WebSite/event/event.component';
import { AboutusComponent } from './WebSite/aboutus/aboutus.component';
import { ActivityComponent } from './WebSite/activity/activity.component';
import { PhotosComponent } from './WebSite/photos/photos.component';
import { ActivityDetailsComponent } from './WebSite/activity-details/activity-details.component';
import { PhotoDetailsComponent } from './WebSite/photo-details/photo-details.component';
import { LoginComponent } from './login/login.component';
import { NotAuthorizedComponent } from './Auth/not-authorized/not-authorized.component';
import { BenefactorLoginComponent } from './WebSite/benefactor/benefactor-login/benefactor-login.component';
import { BenefactorWebDetailsComponent } from './WebSite/benefactor/benefactor-web-details/benefactor-web-details.component';
import { BenefactorauthGuard } from './Auth/benefactorauth.guard';
import { ProjectsComponent } from './WebSite/projects/projects.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'not-authorized', component: NotAuthorizedComponent },
  { path: 'benefactor-login', component: BenefactorLoginComponent },
  { path: 'projects/events/:id', component: ProjectsComponent },
  { path: 'benefactor-details', component: BenefactorWebDetailsComponent, canActivate: [BenefactorauthGuard] },

  { path: 'admin', loadChildren: () => import('../app/Admin/admin.module').then(m => m.AdminModule) },
  {
    path: '', component: HomeComponent, children: [
      { path: 'event', component: EventComponent },
      { path: 'aboutus', component: AboutusComponent },
      { path: 'activity', component: ActivityComponent },
      { path: 'activity-details/:id', component: ActivityDetailsComponent },
      { path: 'photos', component: PhotosComponent },
      { path: 'photo-details/:id', component: PhotoDetailsComponent },
      { path: '', redirectTo: 'activity', pathMatch: 'full' }
    ]
  },
  { path: '', redirectTo: '', pathMatch: 'full' },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
