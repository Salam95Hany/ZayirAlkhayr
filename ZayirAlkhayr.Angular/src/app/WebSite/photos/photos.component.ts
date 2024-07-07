import { Component, OnInit } from '@angular/core';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';

@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css']
})
export class PhotosComponent implements OnInit {
  PhotosData: any[] = [];
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 100
  }

  constructor(private adminService: AdminWebsiteService) { }

  ngOnInit(): void {
    this.GetAllPhotos();

  }

  GetAllPhotos() {
    this.adminService.GetAllPhotos(this.PagingFilter).subscribe(data => {
      this.PhotosData = data.filter(i => i.isVisible);
    });
  }

}
