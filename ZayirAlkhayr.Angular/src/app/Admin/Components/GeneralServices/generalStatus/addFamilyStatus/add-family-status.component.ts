import { Component } from '@angular/core';

@Component({
  selector: 'app-add-family-status',
  templateUrl: './add-family-status.component.html',
  styleUrls: ['./add-family-status.component.css']
})
export class AddFamilyStatusComponent {

  stepsItems = [
    { number: 1, name: 'الحالة' },
    { number: 2, name: 'بيانات الاسرة' },
    { number: 3, name: 'بيانات دخل الاسرة' },
    { number: 4, name: 'بيان المصروفات للاسرة' },
    { number: 5, name: 'الجانب الطبي لأفراد الاسرة' },
    { number: 6, name: 'احتياجات الحالة' },
    { number: 7, name: 'المراجعين' },
  ];

  activeStep = 1;
  onSteps(index: number) {
    if (index === 1) {
      this.activeStep = 1;
    } else if (index === 2) {
      this.activeStep = 2;
    } else if (index === 3) {
      this.activeStep = 3;
    } else if (index === 4) {
      this.activeStep = 4;
    } else if (index === 5) {
      this.activeStep = 5;
    } else if (index === 6) {
      this.activeStep = 6;
    } else if (index === 7) {
      this.activeStep = 7;
    }
  }
  nextStepers() {
    this.activeStep += 1;
  }
  prevStepers() {
    this.activeStep -= 1;
  }
}

