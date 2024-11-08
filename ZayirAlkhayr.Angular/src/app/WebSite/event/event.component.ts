import { Component, OnInit, ViewChild } from '@angular/core';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';
import Swiper from 'swiper';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  EventsData: any[] = [];

  constructor(private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetAllWebSiteEvents();
  }

  GetAllWebSiteEvents() {
    this.adminService.GetAllWebSiteEvents().subscribe(data => {
      this.EventsData = data;
    })
  }

}
