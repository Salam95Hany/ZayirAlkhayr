import { Component, OnInit, ViewChild } from '@angular/core';
import { Slide } from "../media/media.interface";
import { AnimationType } from "../media/media.animations";
import { Router } from '@angular/router';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @ViewChild("navbar") navbarEl: HTMLElement;
  collapsed = true;
  slides: Slide[] = [];
  animationType = AnimationType.Scale;

  constructor(private router: Router, private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetHomeSliderImages();

  }

  GetHomeSliderImages() {
    this.adminService.GetHomeSliderImages().subscribe(data => {
      this.slides = data.map<Slide>(i => { return { headline: i.title, src: i.image } });
    });
  }

  GoToAdmin() {
    this.router.navigateByUrl('/login');
  }
}
