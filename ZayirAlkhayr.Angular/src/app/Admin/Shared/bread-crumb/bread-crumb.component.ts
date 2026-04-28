import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Data, NavigationEnd, PRIMARY_OUTLET, Router } from '@angular/router';
import { Subject, filter, takeUntil } from 'rxjs';

interface BreadCrumbItem {
  label: string;
  link: string | null;
  active: boolean;
}

@Component({
  selector: 'app-bread-crumb',
  templateUrl: './bread-crumb.component.html',
  styleUrls: ['./bread-crumb.component.css']
})
export class BreadCrumbComponent implements OnInit, OnDestroy {
  items: BreadCrumbItem[] = [];
  pageTitle = 'الصفحة الحالية';
  pageSection = 'الموقع الحالي';
  pageHint = 'تنقل واضح داخل صفحات لوحة التحكم.';
  pageIcon = 'uil uil-location-point';

  private readonly destroy$ = new Subject<void>();
  private readonly fallbackBreadcrumbs: Record<string, string[]> = {
    dashboard: ['لوحة التحكم'],
    items: ['النظام', 'إدارة المطعم', 'العناصر'],
    categories: ['النظام', 'إدارة المطعم', 'الفئات'],
    'order-list': ['النظام', 'إدارة المطعم', 'قائمة الطلبات'],
    customers: ['النظام', 'إدارة المطعم', 'العملاء'],
    'inventory-items': ['المخزون', 'إدارة المخزون', 'عناصر المخزون'],
    'inventory-item-recipes': ['المخزون', 'إدارة المخزون', 'وصفات الأصناف'],
    'inventory-suppliers': ['المخزون', 'إدارة المخزون', 'الموردون'],
    'inventory-adjustments': ['المخزون', 'إدارة المخزون', 'حركة المخزون'],
    'inventory-purchases': ['المخزون', 'إدارة المخزون', 'المشتريات'],
    'daily-sales-reports': ['التقارير', 'تقارير المبيعات', 'المبيعات اليومية'],
    'monthly-sales-reports': ['التقارير', 'تقارير المبيعات', 'المبيعات الشهرية'],
    'all-item-reports': ['التقارير', 'تقارير العناصر', 'كل العناصر'],
    'top-sellingitem-reports': ['التقارير', 'تقارير العناصر', 'الأكثر مبيعًا'],
    'lowest-sellingitem-reports': ['التقارير', 'تقارير العناصر', 'الأقل مبيعًا'],
    'never-solditem-reports': ['التقارير', 'تقارير العناصر', 'غير المباعة'],
    users: ['إعدادات النظام', 'الإعدادات', 'المستخدمون'],
    backup: ['إعدادات النظام', 'الإعدادات', 'النسخ الاحتياطية'],
    'factory-reset': ['إعدادات النظام', 'الإعدادات', 'إعادة ضبط المصنع'],
    'user-profile': ['إعدادات النظام', 'الملف الشخصي']
  };

  private readonly sectionMeta: Record<string, { icon: string; hint: string }> = {
    'لوحة التحكم': {
      icon: 'uil uil-apps',
      hint: 'رؤية سريعة ومنظمة لحالة التشغيل والمؤشرات اليومية.'
    },
    'النظام': {
      icon: 'uil uil-store',
      hint: 'إدارة المطعم والطلبات والعملاء ضمن تجربة موحدة وواضحة.'
    },
    'التقارير': {
      icon: 'uil uil-chart-line',
      hint: 'متابعة الأداء والمبيعات والأصناف بواجهة تحليلية متناسقة.'
    },
    'المخزون': {
      icon: 'uil uil-box',
      hint: 'الوصول السريع إلى حركة المخزون والعناصر والموردين.'
    },
    'إعدادات النظام': {
      icon: 'uil uil-cog',
      hint: 'التحكم بالمستخدمين والإعدادات والنسخ الاحتياطية بسهولة.'
    },
    default: {
      icon: 'uil uil-location-point',
      hint: 'تنقل واضح داخل صفحات لوحة التحكم.'
    }
  };

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe(() => this.buildBreadCrumb());

    this.buildBreadCrumb();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  trackByLabel(index: number, item: BreadCrumbItem): string {
    return `${index}-${item.label}`;
  }

  private buildBreadCrumb(): void {
    const trail = this.getResolvedTrail();
    const currentLabel = trail[trail.length - 1] ?? 'الصفحة الحالية';
    const sectionLabel = trail.length > 1 ? trail[0] : 'الرئيسية';
    const metaKey = trail[0] ?? currentLabel;
    const meta = this.sectionMeta[metaKey] ?? this.sectionMeta['default'];

    this.pageTitle = currentLabel;
    this.pageSection = sectionLabel;
    this.pageHint = meta.hint;
    this.pageIcon = meta.icon;

    this.items = [
      {
        label: 'الرئيسية',
        link: '/admin/dashboard',
        active: false
      },
      ...trail.map((label, index) => ({
        label,
        link: null,
        active: index === trail.length - 1
      }))
    ];
  }

  private getResolvedTrail(): string[] {
    const routeData = this.getDeepestRouteData();
    const breadcrumbFromData = this.normalizeBreadCrumb(routeData);

    if (breadcrumbFromData.length) {
      return breadcrumbFromData;
    }

    const normalizedUrl = this.router.url.split('?')[0].split('#')[0];
    const pathSegments = normalizedUrl.split('/').filter(Boolean);
    const routeKey = pathSegments[pathSegments.length - 1] === 'admin'
      ? 'dashboard'
      : pathSegments[pathSegments.length - 1];

    return this.fallbackBreadcrumbs[routeKey] ?? ['الصفحة الحالية'];
  }

  private getDeepestRouteData(): Data {
    let currentRoute = this.activatedRoute.root;

    while (currentRoute.firstChild) {
      currentRoute = currentRoute.firstChild;

      if (currentRoute.outlet !== PRIMARY_OUTLET) {
        continue;
      }
    }

    return currentRoute.snapshot.data ?? {};
  }

  private normalizeBreadCrumb(routeData: Data): string[] {
    const breadcrumb = routeData['breadcrumb'];

    if (Array.isArray(breadcrumb)) {
      return breadcrumb
        .filter((item): item is string => typeof item === 'string')
        .map(item => item.trim())
        .filter(Boolean);
    }

    if (typeof breadcrumb === 'string' && breadcrumb.trim()) {
      return [breadcrumb.trim()];
    }

    return [];
  }
}
