import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FamilyExpenses, FamilyIncome } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';

@Component({
  selector: 'app-family-expenses-data',
  templateUrl: './family-expenses-data.component.html',
  styleUrls: ['./family-expenses-data.component.css']
})
export class FamilyExpensesDataComponent {
  @Input() FamilyMembersCount: number = 0;
  TotalFamilyIncome: number = 0;
  FamilyExpenses: FamilyExpenses = {
    rent_Electricity_Water_Gas_Sewage: 0,
    medicalExamination_Treatment: 0,
    installment_debts: 0,
    schoolExpenses: 0,
    physiotherapySessions: 0,
    analysis: 0,
    satisfactoryTransfers: 0,
    medicalXRays: 0,
    isMinisterialSupply: false,
    isFoodBank: false,
    totalFamilyExpenses: 0,
    netFamilyIncome: 0,
    familyCount: 0
  };

  constructor(private toaster: ToastrService) { }

  InetialData(data: FamilyIncome) {
    this.FamilyExpenses.totalFamilyExpenses = 0;
    this.FamilyExpenses.netFamilyIncome = 0;
    this.FamilyExpenses.familyCount = !this.FamilyMembersCount ? 0 : this.FamilyMembersCount;
    Object.entries(data).filter(([key, value]) => typeof value === 'number').forEach(([key, value]) => {
      this.FamilyExpenses.netFamilyIncome += value;
    });

    this.FamilyExpenses.netFamilyIncome = this.FamilyExpenses.netFamilyIncome - data.totalFamilyIncome;
  }

  onInputChange(value: string, key: any) {
    if (!value)
      return;

    this.FamilyExpenses.totalFamilyExpenses = 0;
    this.FamilyExpenses[key] = value ? Number(value) : 0;
    if (Number(value) > 0)
      Object.entries(this.FamilyExpenses).filter(([key, value]) => typeof value === 'number'
        && key != 'totalFamilyExpenses' && key != 'netFamilyIncome' && key != 'familyCount').forEach(([key, value]) => {
          this.FamilyExpenses.totalFamilyExpenses += value;
        });
  }

  GetOutputData() {
    let arry = Object.entries(this.FamilyExpenses).filter(([key, value]) => typeof value === 'number'
      && key != 'totalFamilyExpenses' && key != 'netFamilyIncome' && key != 'familyCount').map(([key, value]) => value)
    let checked = arry.some(value => value > 0);

    if (!checked) {
      this.toaster.warning('برجاء ادخال قيمة واحدة على الأقل');
      return null;
    } else
      return this.FamilyExpenses;
  }

}
