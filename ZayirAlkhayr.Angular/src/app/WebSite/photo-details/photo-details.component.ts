import { Component, OnInit } from '@angular/core';
import { Slide } from '../media/media.interface';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-photo-details',
  templateUrl: './photo-details.component.html',
  styleUrls: ['./photo-details.component.css']
})
export class PhotoDetailsComponent implements OnInit {
  isGallery = false;
  currentGallery: number = 0;
  PhotoDetails: any;
  PhotoId: any;
  imageUrls: Slide[] = [];


  constructor(private adminService: AdminWebsiteService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.PhotoId = this.route.snapshot.paramMap.get('id');
    this.GetPhotoWithDetailsById();
  }

  GetPhotoWithDetailsById() {
    this.adminService.GetPhotoWithDetailsById(this.PhotoId).subscribe(data => {
      this.PhotoDetails = data;
      this.imageUrls = data.detailImages.map(i => { return { src: i } });
    })
  }
}
