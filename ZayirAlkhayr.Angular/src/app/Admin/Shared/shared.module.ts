import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArabicDateWithTimePipe } from '../Pipes/arabic-date-with-time.pipe';



@NgModule({
  declarations: [
    ArabicDateWithTimePipe
  ],
  imports: [
    CommonModule
  ],
  exports:[ArabicDateWithTimePipe]
})
export class SharedModule { }
