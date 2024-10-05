import { Component } from '@angular/core';
import { FamilyExtraDetails } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';

@Component({
  selector: 'app-reviewers',
  templateUrl: './reviewers.component.html',
  styleUrls: ['./reviewers.component.css']
})
export class ReviewersComponent {
  ExtraDetails: FamilyExtraDetails = {} as FamilyExtraDetails;
  isDate = false;
  PersonalPearpers: any[] = [
    { id: 1, name: 'صور بطاقات الرقم القومي لأفراد الاسرة (جواز سفر لغير المصري)', isSelected: false },
    { id: 2, name: 'صور شهادات الميلاد للقصر', isSelected: false },
    { id: 3, name: 'صورة من عقد الشقة (ايجار - تمليك)', isSelected: false },
    { id: 4, name: 'صورة من قسيمة الزواج', isSelected: false },
    { id: 5, name: 'صورة من الشهادات (الوفاة - الطلاق)', isSelected: false },
    { id: 6, name: 'صور الروشتات والتقارير الطبية', isSelected: false },
    { id: 7, name: 'ايصالات (مياه - كهرباء - غاز)', isSelected: false },
    { id: 8, name: 'صورة من (التأمينات - الشؤون)', isSelected: false },
    { id: 9, name: 'بيان طالب للاولاد بالمدرسة او الحضانة', isSelected: false },
  ];

  GetOutputData() {
    this.ExtraDetails.personalPapers = this.PersonalPearpers.filter(i => i.isSelected).map(i => i.id).join(';;;');
    let isObjEmpty = Object.keys(this.ExtraDetails).every(key => !this.ExtraDetails[key]);
    if (isObjEmpty)
      return {};
    else
      return this.ExtraDetails;
  }


  onfocus() {
    this.isDate = true;
  }
}
