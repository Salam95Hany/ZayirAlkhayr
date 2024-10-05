import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FamilyStatus } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { FamilyCategories, FamilyNationalities, FamilyStatusTypes } from 'src/app/Admin/Models/GeneralStatus/FamilyStatusLookups';

@Component({
  selector: 'app-family-status',
  templateUrl: './family-status.component.html',
  styleUrls: ['./family-status.component.css']
})
export class FamilyStatusComponent implements OnInit {
  @ViewChild('FamilyForm') FamilyForm: any;
  @Input() Categories: FamilyCategories[] = [];
  @Input() Nationalities: FamilyNationalities[] = [];
  @Input() StatusTypes: FamilyStatusTypes[] = [];
  FamilyStatus: FamilyStatus = {} as FamilyStatus;
  UserModel: any;
  isDate = false;

  constructor() {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FamilyStatus.nationalityId = null;
    this.FamilyStatus.categoryId = null;
    this.FamilyStatus.statusTypeId = null;
    this.FamilyStatus.insertUser = this.UserModel?.userId;
  }

  onfocus() {
    this.isDate = true;
  }

  GetOutputData() {
    this.FamilyForm.onSubmit();

    const isValid = this.FamilyForm.form.valid;
    if (!isValid)
      return null;
    else
      return this.FamilyStatus;
  }
}
