import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

interface MenuItem {
  title: string;
  route: string;
}

interface MenuGroup {
  id: string;
  title: string;
  icon: string;
  collapsed: boolean;
  items: MenuItem[];
}

interface MenuSection {
  title: string;
  roles?: string[];
  groups: MenuGroup[];
}

@Component({
  selector: 'app-admin-side-menu',
  templateUrl: './admin-side-menu.component.html',
  styleUrls: ['./admin-side-menu.component.css']
})
export class AdminSideMenuComponent implements OnInit {
  @Input() isCollapsing = false;
  @Output() closeSideMenuFromOverlayEvent = new EventEmitter<boolean>();
  UserModel: any;
  RoleName = '';
  Roles = [
    { nameEn: 'Admin', nameAr: 'مدير' },
    { nameEn: 'Cashier', nameAr: 'كاشير' }
  ];
  menuSections: MenuSection[] = [
    {
      title: 'النظام',
      roles: ['Admin'],
      groups: [
        {
          id: 'restaurant',
          title: 'إدارة المطعم',
          icon: 'uil-store',
          collapsed: true,
          items: [
            { title: 'الفئات', route: 'categories' },
            { title: 'العناصر', route: 'items' },
            { title: 'الطاولات', route: 'tables' },
            { title: 'قائمة الطلبات', route: 'order-list' },
            { title: 'العملاء', route: 'customers' },
            { title: 'إنشاء طلب', route: '/create-order' }
          ]
        },
        {
          id: 'employees',
          title: 'إدارة الموظفين',
          icon: 'uil-users-alt',
          collapsed: true,
          items: [
            { title: 'الوظائف', route: 'jop-title' },
            { title: 'الموظفين', route: 'employees' },
            { title: 'رواتب الموظفين', route: 'employee-salary' }
          ]
        },
        {
          id: 'expenses',
          title: 'إدارة المصاريف',
          icon: 'uil-money-withdraw',
          collapsed: true,
          items: [
            { title: 'أنواع المصاريف', route: 'expenses-types' },
            { title: 'المصاريف', route: 'expenses' }
          ]
        }
      ]
    },

    {
      title: 'المخزون',
      roles: ['Admin'],
      groups: [
        {
          id: 'inventory',
          title: 'إدارة المخزون',
          icon: 'uil-box',
          collapsed: true,
          items: [
            { title: 'الوحدات', route: 'inventory-units' },
            { title: 'عناصر المخزون', route: 'inventory-items' },
            { title: 'وصفات الأصناف', route: 'inventory-item-recipes' },
            { title: 'حركة المخزون', route: 'inventory-adjustments' }
          ]
        },
        {
          id: 'purchase',
          title: 'إدارة المشتريات',
          icon: 'uil-truck',
          collapsed: true,
          items: [
            { title: 'المشتريات', route: 'inventory-purchases' },
            { title: 'الموردون', route: 'inventory-suppliers' }
          ]
        }
      ]
    },

    {
      title: 'التقارير',
      roles: ['Admin'],
      groups: [
        {
          id: 'salesReports',
          title: 'تقارير المبيعات',
          icon: 'uil-chart-line',
          collapsed: true,
          items: [
            { title: 'المبيعات اليومية', route: 'daily-sales-reports' },
            { title: 'المبيعات الشهرية', route: 'monthly-sales-reports' }
          ]
        },
        {
          id: 'itemsReports',
          title: 'تقارير العناصر',
          icon: 'uil-chart-line',
          collapsed: true,
          items: [
            { title: 'كل العناصر', route: 'all-item-reports' },
            { title: 'الأكثر مبيعًا', route: 'top-sellingitem-reports' },
            { title: 'الأقل مبيعًا', route: 'lowest-sellingitem-reports' },
            { title: 'الغير مباعة', route: 'never-solditem-reports' }
          ]
        },
        {
          id: 'financeReports',
          title: 'تقارير المالية',
          icon: 'uil-chart-line',
          collapsed: true,
          items: [
            { title: 'المصاريف الشهرية', route: 'expenses-monthly-reports' },
            { title: 'الرواتب الشهرية', route: 'employee-salary-reports' }
          ]
        }
      ]
    },

    {
      title: 'إعدادات النظام',
      roles: ['Admin'],
      groups: [
        {
          id: 'settings',
          title: 'الإعدادات',
          icon: 'uil-cog',
          collapsed: true,
          items: [
            { title: 'المستخدمون', route: 'users' },
            { title: 'النسخ الاحتياطية', route: 'backup' },
            { title: 'إعادة ضبط المصنع', route: 'factory-reset' }
          ]
        }
      ]
    }
  ];

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') || '{}');
    this.RoleName = this.Roles.find(x => x.nameEn === this.UserModel?.role)?.nameAr ?? '';
    this.expandCurrentGroup();
  }

  private expandCurrentGroup(): void {
    const currentRoute = this.router.url.split('/')[2];
    this.menuSections.forEach(section => {
      section.groups.forEach(group => {
        group.collapsed = !group.items.some(item => {
          if (item.route.startsWith('/')) {
            return this.router.url === item.route;
          }

          return item.route === currentRoute;
        });
      });
    });
  }

  toggle(group: MenuGroup): void {
    group.collapsed = !group.collapsed;
  }

  onCloseSidemenuFromOverlay(): void {
    this.closeSideMenuFromOverlayEvent.emit();
  }

}