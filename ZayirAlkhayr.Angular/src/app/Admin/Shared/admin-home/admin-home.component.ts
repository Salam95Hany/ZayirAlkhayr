import { Component, OnInit } from '@angular/core';
import { AdminWebsiteService } from '../../Services/admin-website.service';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css']
})
export class AdminHomeComponent implements OnInit {
  UsersData: any[] = [];
  Roles = [
    { nameEn: 'SupperAdmin', nameAr: 'مدير' },
    { nameEn: 'WebSite', nameAr: 'موقع زائر الخير' },
    { nameEn: 'Services', nameAr: 'خدمات اجتماعية' },
    { nameEn: 'BeneFactors', nameAr: 'متبرعين' },
    { nameEn: 'Accounts', nameAr: 'حسابات' },
    { nameEn: 'Admin', nameAr: 'مشرف' }
  ];
  StatisticsData: any;
  TotalCount = 0;

  constructor(private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.GetStatisticsHome();
    this.GetAllUsers();
  }

  GetStatisticsHome() {
    this.adminService.GetStatisticsHome().subscribe(data => {
      this.StatisticsData = data;
    })
  }

  GetAllUsers() {
    this.adminService.GetAllUsers().subscribe(data => {
      this.UsersData = data.filter(i => i.isActive);
      this.UsersData.forEach(item => {
        let role = this.Roles.find(i => i.nameEn == item.role);
        if (role)
          item.role = role.nameAr;
      });
      this.TotalCount = this.UsersData.length;
    })
  }

}
