import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArabicDateWithTimePipe } from '../Pipes/arabic-date-with-time.pipe';
import { ArabicDatePipe } from '../Pipes/arabic-date.pipe';
import { RoleCheckerDirective } from '../Directives/role-checker.directive';



@NgModule({
  declarations: [
    ArabicDateWithTimePipe,
    ArabicDatePipe,
    RoleCheckerDirective
  ],
  imports: [
    CommonModule
  ],
  exports:[ArabicDateWithTimePipe,ArabicDatePipe,RoleCheckerDirective]
})
export class SharedModule { }
