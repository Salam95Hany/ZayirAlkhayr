import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FamilyStatus } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { FamilyCategories, FamilyNationalities, FamilyStatusTypes } from 'src/app/Admin/Models/GeneralStatus/FamilyStatusLookups';

@Component({
  selector: 'app-family-status',
  templateUrl: './family-status.component.html',
  styleUrls: ['./family-status.component.css']
})
export class FamilyStatusComponent implements OnInit {
  @ViewChild('FamilyForm') FamilyForm: any;
  @Output() StatusNameChanged = new EventEmitter<string>();
  @Input() FamilyStatus: FamilyStatus = {} as FamilyStatus;
  @Input() Categories: FamilyCategories[] = [];
  @Input() Nationalities: FamilyNationalities[] = [];
  @Input() StatusTypes: FamilyStatusTypes[] = [];
  @Input() UpdateMode = false;
  @Input() DetailsMode = false;
  UserModel: any;
  isDate = false;

  constructor(private datePipe: DatePipe) { }

  ngOnInit(): void {
    debugger
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    if (!this.UpdateMode && !this.DetailsMode) {
      this.FamilyStatus.nationalityId = null;
      this.FamilyStatus.categoryId = null;
      this.FamilyStatus.statusTypeId = null;
    } else {
      this.isDate = this.FamilyStatus.addedDate ? true : false;
      this.FamilyStatus.addedDate = this.datePipe.transform(this.FamilyStatus.addedDate, 'yyyy-MM-dd');
    }

    this.FamilyStatus.insertUser = this.UserModel?.userId;
  }

  FamilyStatusNameChange() {
    if (this.UpdateMode)
      this.StatusNameChanged.emit(this.FamilyStatus.name);
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
