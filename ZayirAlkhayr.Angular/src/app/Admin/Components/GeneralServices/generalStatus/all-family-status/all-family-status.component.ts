import { Component, OnInit } from '@angular/core';
import { FilterModel } from 'src/app/Admin/Models/FilterModel';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { GeneralStatusService } from 'src/app/Admin/Services/general-status.service';

@Component({
  selector: 'app-all-family-status',
  templateUrl: './all-family-status.component.html',
  styleUrls: ['./all-family-status.component.css']
})
export class AllFamilyStatusComponent implements OnInit {
  FamilyStatusData: any[] = [];
  FilterList: FilterModel[] = [];
  showLoader = false;
  isFilter = false;
  TotalCount = 0;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 20
  }

  constructor(private generalService: GeneralStatusService) {

  }

  ngOnInit(): void {
    this.GetAllFamilyStatusData();
    this.GetAllFamilyStatusFilter();
  }

  GetAllFamilyStatusData() {
    this.generalService.GetAllFamilyStatusData(this.PagingFilter).subscribe(data => {
      this.FamilyStatusData = data;
      this.TotalCount = this.FamilyStatusData && this.FamilyStatusData.length > 0 ? this.FamilyStatusData[0].totalCount : 0;
    });
  }

  GetAllFamilyStatusFilter() {
    this.generalService.GetAllFamilyStatusFilter(this.PagingFilter).subscribe(data => {
      this.FilterList = data;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetAllFamilyStatusData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetAllFamilyStatusData();
  }
}
