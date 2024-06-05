import { Component, OnInit, TemplateRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../Services/admin-website.service';

@Component({
  selector: 'app-admin-activity',
  templateUrl: './admin-activity.component.html',
  styleUrls: ['./admin-activity.component.css']
})
export class AdminActivityComponent implements OnInit {
  isFilter = false;
  ActivitiesData: any[] = [];

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService) { }

  ngOnInit(): void {
    this.GetAllActivities();
  }

  openAddItemModal(content: TemplateRef<any>) {
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    })
  }

  openDeleteItemModal(content: TemplateRef<any>) {
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    })
  }

  GetAllActivities() {
    this.adminService.GetAllActivities().subscribe(data => {
      this.ActivitiesData = data;
    })
  }
}
