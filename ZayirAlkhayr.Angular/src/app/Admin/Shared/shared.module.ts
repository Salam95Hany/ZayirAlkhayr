import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArabicDateWithTimePipe } from '../Pipes/arabic-date-with-time.pipe';
import { ArabicDatePipe } from '../Pipes/arabic-date.pipe';



@NgModule({
  declarations: [
    ArabicDateWithTimePipe,
    ArabicDatePipe
  ],
  imports: [
    CommonModule
  ],
  exports:[ArabicDateWithTimePipe,ArabicDatePipe]
})
export class SharedModule { }
