import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArabicDateWithTimePipe } from '../Pipes/arabic-date-with-time.pipe';
import { ArabicDatePipe } from '../Pipes/arabic-date.pipe';
import { RoleCheckerDirective } from '../Directives/role-checker.directive';
import { AdminGeneralInputComponent } from './admin-general-input/admin-general-input.component';
import { AdminDropDownComponent } from './admin-drop-down/admin-drop-down.component';
import { SearchArryPipe } from '../Pipes/search-arry.pipe';
import { NgbModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';



@NgModule({
  declarations: [
    ArabicDateWithTimePipe,
    ArabicDatePipe,
    RoleCheckerDirective,
    AdminGeneralInputComponent,
    AdminDropDownComponent,
    SearchArryPipe
  ],
  imports: [
    CommonModule,
    NgbNavModule,
    NgbModule
  ],
  exports: [ArabicDateWithTimePipe, ArabicDatePipe, RoleCheckerDirective, SearchArryPipe,AdminGeneralInputComponent,AdminDropDownComponent]
})
export class SharedModule { }
