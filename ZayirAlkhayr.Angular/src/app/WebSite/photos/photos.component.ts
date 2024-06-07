import { Component, OnInit } from '@angular/core';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css']
})
export class PhotosComponent implements OnInit {
  PhotosData: any[] = [];
  constructor(private adminService: AdminWebsiteService) { }

  ngOnInit(): void {
    this.GetAllPhotos();

  }

  GetAllPhotos() {
    this.adminService.GetAllPhotos().subscribe(data => {
      this.PhotosData = data.filter(i => i.isVisible);
    });
  }

}
