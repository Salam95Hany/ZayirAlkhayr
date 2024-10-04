import { Component, Input, OnInit } from '@angular/core';
import { FamilyDetails } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';

@Component({
  selector: 'app-family-medical',
  templateUrl: './family-medical.component.html',
  styleUrls: ['./family-medical.component.css']
})
export class FamilyMedicalComponent {
  @Input() FamilyDetails: FamilyDetails[] = [];
  @Input() FamilyStatusName: string;
  FamilyNames: string[] = [];

  InetialData() {
    this.FamilyNames = [];
    this.FamilyNames.push(this.FamilyStatusName);
    this.FamilyNames.push(...this.FamilyDetails.map(i => i.name));
  }


  GetOutputData() {
    return {};
  }
  isDate = false;
  onfocus() {
    this.isDate = true;
  }
}
