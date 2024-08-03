import { Component, OnInit } from '@angular/core';
import { PagingFilterModel } from '../../../Models/PagingFilterModel';
import { AdminWebsiteService } from '../../../Services/admin-website.service';
import { FilterModel } from '../../../Models/FilterModel';

@Component({
  selector: 'app-bene-factor-notes',
  templateUrl: './bene-factor-notes.component.html',
  styleUrls: ['./bene-factor-notes.component.css']
})
export class BeneFactorNotesComponent implements OnInit {
  BeneFactorNotes: any[] = [];
  isFilter = false;
  TotalCount = 0;
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 10
  }

  constructor(private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetBeneFactorNotes();
  }

  GetBeneFactorNotes() {
    this.adminService.GetBeneFactorNotes(this.PagingFilter).subscribe(data => {
      this.BeneFactorNotes = data;
      this.TotalCount = this.BeneFactorNotes && this.BeneFactorNotes.length > 0 ? this.BeneFactorNotes[0].totalCount : 0;
    });
  }

  PageChange(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetBeneFactorNotes();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetBeneFactorNotes();
  }

}
