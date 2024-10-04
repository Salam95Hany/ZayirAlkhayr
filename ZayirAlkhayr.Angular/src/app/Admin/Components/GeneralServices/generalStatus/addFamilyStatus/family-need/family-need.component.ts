import { Component, Input, OnInit } from '@angular/core';
import { FamilyNeeds } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { FamilyNeedCategoryGroups, FamilyNeedTypes } from 'src/app/Admin/Models/GeneralStatus/FamilyStatusLookups';

@Component({
  selector: 'app-family-need',
  templateUrl: './family-need.component.html',
  styleUrls: ['./family-need.component.css']
})
export class FamilyNeedComponent implements OnInit {
  @Input() FamilyNeeds: FamilyNeedCategoryGroups[] = [];
  SelectedNeeds: FamilyNeeds[] = [];
  isDate = false;

  constructor() { }

  ngOnInit(): void { }

  AddFamilyNeed(type: FamilyNeedCategoryGroups, item: FamilyNeedTypes) {
    let obj = type.selectedNeeds.find(i => i.needTypeId == item.id);
    if (!obj)
      type.selectedNeeds.push({ needTypeId: item.id, name: item.name });
  }

  RemoveSelectedNeed(type: FamilyNeedCategoryGroups, index: number) {
    type.selectedNeeds.splice(index, 1);
  }


  GetOutputData() {
    this.SelectedNeeds = [];
    this.FamilyNeeds.forEach(item => {
      item.selectedNeeds.forEach(need => {
        this.SelectedNeeds.push({ needTypeId: need.needTypeId });
      });
    });
    return this.SelectedNeeds;
  }

  onfocus() {
    this.isDate = true;
  }

}
