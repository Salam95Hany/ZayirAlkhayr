import { Component, OnInit, ViewChild } from '@angular/core';
import { FamilyStatusLookups } from 'src/app/Admin/Models/GeneralStatus/FamilyStatusLookups';
import { GeneralStatusService } from 'src/app/Admin/Services/general-status.service';
import { FamilyStatusComponent } from './family-status/family-status.component';
import { FamilyDataComponent } from './family-data/family-data.component';
import { FamilyIncomeDataComponent } from './family-income-data/family-income-data.component';
import { FamilyExpensesDataComponent } from './family-expenses-data/family-expenses-data.component';
import { FamilyMedicalComponent } from './family-medical/family-medical.component';
import { FamilyNeedComponent } from './family-need/family-need.component';
import { ReviewersComponent } from './reviewers/reviewers.component';
import { AddFamilyStatusModel } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-family-status',
  templateUrl: './add-family-status.component.html',
  styleUrls: ['./add-family-status.component.css']
})
export class AddFamilyStatusComponent implements OnInit {
  @ViewChild('FamilyStatus') FamilyStatus: FamilyStatusComponent;
  @ViewChild('FamilyData') FamilyData: FamilyDataComponent;
  @ViewChild('FamilyIncome') FamilyIncome: FamilyIncomeDataComponent;
  @ViewChild('FamilyExpenses') FamilyExpenses: FamilyExpensesDataComponent;
  @ViewChild('FamilyMedical') FamilyMedical: FamilyMedicalComponent;
  @ViewChild('FamilyNeed') FamilyNeed: FamilyNeedComponent;
  @ViewChild('Reviewers') Reviewers: ReviewersComponent;
  FamilyLookups: FamilyStatusLookups = {} as FamilyStatusLookups;
  AddFamilyStatusModel: AddFamilyStatusModel = {} as AddFamilyStatusModel;
  showLoader = false;
  StepName: string;
  activeStep = 1;
  Counter = 0;
  viewChilds = [];
  Steps: any[] = [];
  StepList: any[] = [
    { stepName: 'الحالة', stepId: 'familyStatus', number: 1 },
    { stepName: 'بيانات الاسرة', stepId: 'familyDetails', number: 2 },
    { stepName: 'بيانات دخل الاسرة', stepId: 'familyIncome', number: 3 },
    { stepName: 'بيان المصروفات للاسرة', stepId: 'familyExpenses', number: 4 },
    { stepName: 'الجانب الطبي لأفراد الاسرة', stepId: 'familyPatient', number: 5 },
    { stepName: 'احتياجات الحالة', stepId: 'familyNeeds', number: 6 },
    { stepName: 'المراجعين', stepId: 'familyExtraDetails', number: 7 }
  ]

  constructor(private generalStatusService: GeneralStatusService, private toaster: ToastrService,
    private router:Router
  ) {
    this.Steps = this.StepList.map(a => a.stepId);
    this.StepName = this.Steps[0];
  }

  ngOnInit(): void {
    this.GetFamilyStatusLookups();
  }

  GetFamilyStatusLookups() {
    this.showLoader = true;
    this.generalStatusService.GetFamilyStatusLookups().subscribe(data => {
      this.showLoader = false;
      this.FamilyLookups = data;
    });
  }

  NextStep() {
    this.viewChilds = [this.FamilyStatus, this.FamilyData, this.FamilyIncome, this.FamilyExpenses, this.FamilyMedical, this.FamilyNeed, this.Reviewers]
    let data = this.viewChilds[this.Counter].GetOutputData();
    if (this.Counter == 0) {
      if (data == null)
        return;
      this.showLoader = true;
      //this.customerService.CheckCustomerIsExist(data?.customerName).subscribe(res => {
      this.showLoader = false;
      // if (!res) {
      this.AddFamilyStatusModel[this.StepName] = data;
      this.Counter++;
      this.activeStep++;
      this.StepName = this.Steps[this.Counter];
      this.AddFamilyStatusModel = { ...this.AddFamilyStatusModel };
      if (this.viewChilds[this.Counter]?.InetialData)
        this.viewChilds[this.Counter].InetialData(data);
      // } else
      //   this.toaster.error(`The Customer ${data?.customerName} Is Exist`);
      //});
    } else {
      if (data !== null) {
        this.AddFamilyStatusModel[this.StepName] = data;
        this.Counter++;
        this.activeStep++;
        this.StepName = this.Steps[this.Counter];
        this.AddFamilyStatusModel = { ...this.AddFamilyStatusModel };
        if (this.viewChilds[this.Counter]?.InetialData)
          this.viewChilds[this.Counter].InetialData(data);
      }
    }
  }

  PreviousSteps() {
    if (this.Counter == 0) {
      return;
    } else {
      this.Counter--;
      this.activeStep--;
      this.StepName = this.Steps[this.Counter];
    }
  }

  OnStepClick(stepName: string, counter: number) {
    const stepData = this.AddFamilyStatusModel[stepName];
    if (stepData) {
      this.StepName = stepName;
      this.Counter = counter;
      this.activeStep = counter + 1;
    }
  }


  AddNewFamilyStatus() {
    let data = this.viewChilds[this.Counter].GetOutputData();
    this.AddFamilyStatusModel[this.StepName] = data;
    this.showLoader = true;
    this.generalStatusService.AddNewFamilyStatus(this.AddFamilyStatusModel).subscribe(data => {
      this.showLoader = false;
      if (data.done) {
        this.toaster.success(data.message);
        this.router.navigateByUrl('/admin/family-status');
      } else
        this.toaster.error(data.message);
    });
  }
}

