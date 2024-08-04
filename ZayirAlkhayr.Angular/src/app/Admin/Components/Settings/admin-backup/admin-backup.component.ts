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

  constructor(private adminService: AdminService, private toaster: ToastrService, private datepipe: DatePipe) { }

  SaveDbBackupFile() {
    this.showLoader = true;
    this.adminService.SaveDbBackupFile().subscribe(data => {
      if (data.done)
        this.toaster.success(data.message);
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

  DownloadZipFile(folderName: string) {
    this.showLoader = true;
    this.adminService.DownloadZipFile(folderName).subscribe(data => {
      if (data.done)
        this.toaster.success(data.message);
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }

}
