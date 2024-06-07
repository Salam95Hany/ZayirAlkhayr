import { Component, OnInit } from '@angular/core';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrls: ['./activity.component.css']
})
export class ActivityComponent implements OnInit {
  ActivitiesData: any[] = [];
  constructor(private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetAllActivities();
  }

  GetAllActivities() {
    this.adminService.GetAllActivities().subscribe(data => {
      this.ActivitiesData = data;
      this.ActivitiesData = this.ActivitiesData.filter(i => i.isVisible);
      console.log(this.ActivitiesData);
    });
  }

}
