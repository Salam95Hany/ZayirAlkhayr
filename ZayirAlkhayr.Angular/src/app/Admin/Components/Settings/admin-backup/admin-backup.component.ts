import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/Admin/Services/admin.service';

@Component({
  selector: 'app-admin-backup',
  templateUrl: './admin-backup.component.html',
  styleUrls: ['./admin-backup.component.css']
})
export class AdminBackupComponent {
  showLoader = false;
  FolderNames = [
    { nameEn: "ActivityImages", nameAr: "الأنشطة" },
    { nameEn: "ActivitySliderImages", nameAr: "تفاصيل الأنشطة" },
    { nameEn: "BeneFactorDetailsImages", nameAr: "تفاصيل المتبرعين" },
    { nameEn: "BeneFactorImages", nameAr: "المتبرعين" },
    { nameEn: "EventSliderImages", nameAr: "الفعاليات" },
    { nameEn: "PhotoDetailImages", nameAr: "تفاصيل الصور" },
    { nameEn: "PhotoImages", nameAr: "الصور" },
    { nameEn: "SliderImages", nameAr: "شريط الصور" }
  ]

  constructor(private adminService: AdminService, private datepipe: DatePipe) { }

  DownloadBackupFile() {
    let today = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = 'ZAbk' + '_' + today;
    this.showLoader = true;
    this.adminService.DownloadBackupFile(fileName).subscribe(data => {
      this.showLoader = false;
    });
  }

  DownloadZipFile(FolderName: string) {
    let folderName = this.FolderNames.find(i => i.nameEn == FolderName).nameAr;
    let today = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
    let fileName = folderName + '_' + today;
    this.showLoader = true;
    this.adminService.DownloadZipFile(FolderName, fileName).subscribe(data => {
      this.showLoader = false;
    });
  }

}
