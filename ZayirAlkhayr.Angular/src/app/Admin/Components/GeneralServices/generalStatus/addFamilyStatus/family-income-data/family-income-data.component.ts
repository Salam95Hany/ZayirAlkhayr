import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FamilyIncome } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';

@Component({
  selector: 'app-family-income-data',
  templateUrl: './family-income-data.component.html',
  styleUrls: ['./family-income-data.component.css']
})
export class FamilyIncomeDataComponent {
  FamilyIncome: FamilyIncome = {
    fatherJop: 0,
    motherJop: 0,
    childernsJop: 0,
    affairSpension_SocialSolidarity: 0,
    project: 0,
    liveStock_Lands: 0,
    organization_ZakatCommittee: 0,
    insurancePension: 0,
    other: 0,
    totalFamilyIncome: 0,
    comments: ''
  };

  constructor(private toaster: ToastrService) { }

  onInputChange(value: string, key: any) {
    if (!value)
      return;
    
    this.FamilyIncome.totalFamilyIncome = 0;
    this.FamilyIncome[key] = value ? Number(value) : 0;
    if (Number(value) > 0)
      Object.entries(this.FamilyIncome).filter(([key, value]) => typeof value === 'number').forEach(([key, value]) => {
        this.FamilyIncome.totalFamilyIncome += value;
      });
  }

  GetOutputData() {
    // let arry = Object.entries(this.FamilyIncome).filter(([key, value]) => typeof value === 'number').map(([key, value]) => value)
    // let checked = arry.some(value => value > 0);

    // if (!checked) {
    //   this.toaster.warning('برجاء ادخال قيمة واحدة على الأقل');
    //   return null;
    // } else
      return this.FamilyIncome;
  }

}
