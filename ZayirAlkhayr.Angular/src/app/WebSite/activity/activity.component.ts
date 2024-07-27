import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  constructor(private adminService: AdminWebsiteService,private modalService: NgbModal) {

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

  OpenVodaFoneCashModal(content: any) {
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  OpenCashModal(content: any) {
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  OpenAhlyBankModal(content: any) {
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

}
