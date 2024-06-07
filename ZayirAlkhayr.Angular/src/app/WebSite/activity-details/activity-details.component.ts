import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';
import SwiperCore, { Autoplay, Navigation, Pagination, Scrollbar, A11y } from 'swiper';
SwiperCore.use([Autoplay, Navigation, Pagination, Scrollbar, A11y]);

@Component({
  selector: 'app-activity-details',
  templateUrl: './activity-details.component.html',
  styleUrls: ['./activity-details.component.css']
})
export class ActivityDetailsComponent implements OnInit {
  ShowInput = false;
  AvtivityData: any;
  ActivityId: any;
  constructor(private modalService: NgbModal, private route: ActivatedRoute, private adminService: AdminWebsiteService) { }

  ngOnInit(): void {
    this.ActivityId = this.route.snapshot.paramMap.get('id');
    this.GetActivityWithSliderImagesById();
  }

  openDonateModal(modal: TemplateRef<any>) {
    this.modalService.open(modal, {
      size: 'xl',
      scrollable: true
    });
  }

  GetActivityWithSliderImagesById() {
    this.adminService.GetActivityWithSliderImagesById(this.ActivityId).subscribe(data => {
      this.AvtivityData = data;
    })
  }
}
