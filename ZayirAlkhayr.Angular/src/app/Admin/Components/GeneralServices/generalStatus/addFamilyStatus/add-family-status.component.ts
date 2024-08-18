import { Component } from '@angular/core';

@Component({
  selector: 'app-add-family-status',
  templateUrl: './add-family-status.component.html',
  styleUrls: ['./add-family-status.component.css']
})
export class AddFamilyStatusComponent {
  activeStep = 1;
  currentStep = 0;
  onSteps(index: number) {
    if (index === 1) {
      this.activeStep = 1;
      this.currentStep = 1;
    } else if (index === 2) {
      this.activeStep = 2;
      this.currentStep = 2;
    } else if (index === 3) {
      this.activeStep = 3;
      this.currentStep = 3;
    } else if (index === 4) {
      this.activeStep = 4;
      this.currentStep = 4;
    } else if (index === 5) {
      this.activeStep = 5;
      this.currentStep = 5;
    } else if (index === 6) {
      this.activeStep = 6;
      this.currentStep = 6;
    } else if (index === 7) {
      this.activeStep = 7;
      this.currentStep = 7;
    }
  }


  nextStepers() {
    this.activeStep += 1;
    this.currentStep += 1;
  }
  prevStepers() {
    this.activeStep -= 1;
    this.currentStep -= 1;
  }
}

