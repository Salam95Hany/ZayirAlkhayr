import { Component } from '@angular/core';

@Component({
  selector: 'app-family-medical',
  templateUrl: './family-medical.component.html',
  styleUrls: ['./family-medical.component.css']
})
export class FamilyMedicalComponent {

  GetOutputData() {
    return {};
  }
  isDate = false;
  onfocus() {
    this.isDate = true;
  }
}
