import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './WebSite/home/home.component';
import { EventComponent } from './WebSite/event/event.component';
import { AboutusComponent } from './WebSite/aboutus/aboutus.component';
import { ActivityComponent } from './WebSite/activity/activity.component';
import { ActivityDetailsComponent } from './WebSite/activity-details/activity-details.component';
import { PhotosComponent } from './WebSite/photos/photos.component';
import { PhotoDetailsComponent } from './WebSite/photo-details/photo-details.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    EventComponent,
    AboutusComponent,
    ActivityComponent,
    ActivityDetailsComponent,
    PhotosComponent,
    PhotoDetailsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
