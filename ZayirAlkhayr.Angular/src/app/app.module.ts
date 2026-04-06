import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SwiperModule } from 'swiper/angular';
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { ngxLoadingAnimationTypes, NgxLoadingModule } from "ngx-loading";
import { NotAuthorizedComponent } from './Auth/not-authorized/not-authorized.component';
import { CommonModule, DatePipe } from '@angular/common';
import { CreateOrderComponent } from './Admin/Shared/create-order/create-order.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NotAuthorizedComponent,
    CreateOrderComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgbModule,
    SwiperModule,
    ToastrModule.forRoot({
      preventDuplicates: true
    }),
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.circleSwish,
      backdropBackgroundColour: 'rgba(0, 18, 59, 0.6)',
      backdropBorderRadius: '3px',
      primaryColour: '#74c173',
      fullScreenBackdrop: true
    }),
    ReactiveFormsModule,
    FormsModule
  ],
  providers:[DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
