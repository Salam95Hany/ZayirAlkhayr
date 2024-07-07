import { Component, OnInit } from '@angular/core';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrls: ['./activity.component.css']
})
export class ActivityComponent implements OnInit {
  ActivitiesData: any[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 100
  }
  constructor(private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetAllActivities();
  }

  GetAllActivities() {
    this.adminService.GetAllActivities(this.PagingFilter).subscribe(data => {
      this.ActivitiesData = data;
      this.ActivitiesData = this.ActivitiesData.filter(i => i.isVisible);
    });
  }

}
