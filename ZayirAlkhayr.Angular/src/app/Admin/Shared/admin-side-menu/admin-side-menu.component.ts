import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-side-menu',
  templateUrl: './admin-side-menu.component.html',
  styleUrls: ['./admin-side-menu.component.css']
})
export class AdminSideMenuComponent implements OnInit {
  @Input() isCollapsing = false;
  @Output() closeSideMenuFromOverlayEvent = new EventEmitter<boolean>();
  isCollapsed_1 = true;
  isCollapsed_2 = true;
  isCollapsed_3 = true;
  RoleName = '';
  UserModel: any;
  Roles = [
    { nameEn: 'SupperAdmin', nameAr: 'المدير' },
    { nameEn: 'WebSite', nameAr: 'موقع زائر الخير' },
    { nameEn: 'Services', nameAr: 'خدمات اجتماعية' },
    { nameEn: 'BeneFactors', nameAr: 'المتبرعين' },
    { nameEn: 'Accounts', nameAr: 'الحسابات' }
  ];
  WebSite = ['home-slideimage', 'activity', 'event', 'photo'];
  BeneFactor = ['benefactors', 'benefactor-detail', 'benefactor-type',
    'benefactor-note', 'account-import-money', 'account-export-money', 'benefactor-nationality'
  ];

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.RoleName = this.Roles.find(i => i.nameEn == this.UserModel?.role)?.nameAr;
    let url = this.router.url.split('/')[2];
    if (this.WebSite.includes(url))
      this.isCollapsed_1 = false;
    else if (this.BeneFactor.includes(url))
      this.isCollapsed_3 = false;
  }

  onCloseSidemenuFromOverlay() {
    this.closeSideMenuFromOverlayEvent.emit();
  }
}
