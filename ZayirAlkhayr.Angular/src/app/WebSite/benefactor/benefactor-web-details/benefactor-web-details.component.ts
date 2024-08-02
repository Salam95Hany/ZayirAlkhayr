import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { PagingFilterModel } from 'src/app/Admin/Models/PagingFilterModel';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';

@Component({
  selector: 'app-benefactor-web-details',
  templateUrl: './benefactor-web-details.component.html',
  styleUrls: ['./benefactor-web-details.component.css']
})
export class BenefactorWebDetailsComponent implements OnInit {
  BeneFactorDetailsData: any[] = [];
  BeneFactorTypeIds: any[] = [];
  BeneFactorTypes: any[] = [];
  BeneFactorValues: any[] = [];
  BeneFactorStatistics: any;
  collapsed = true;
  FirstLoad = true;
  showLoader = false;
  BeneFactorModel: any;
  BeneFactorId: any;
  BeneFactorValueId: any;
  BeneFactorTypeId = 0;
  BeneFactorTypeName = 'اختر نوع التبرع';
  Type = '';
  TotalValue = 0;
  DefaultImage = '../../../../assets/logo-2.png';
  ImageSrc: any;
  Notes = '';
  Suggestions = '';
  PagingFilter: PagingFilterModel = {
    filterList: [],
    currentpage: 1,
    pagesize: 100
  }
  constructor(private router: Router, private adminService: AdminWebsiteService, private offcanvasService: NgbOffcanvas,
    private modalService: NgbModal, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.BeneFactorModel = JSON.parse(localStorage.getItem('BeneFactorModel'));
    this.BeneFactorId = this.BeneFactorModel?.beneFactorId;
    if (this.BeneFactorModel?.welcomeMessage)
      this.toaster.info(this.BeneFactorModel?.welcomeMessage, `مرحبا ${this.BeneFactorModel?.name}`, {
        progressBar: true,
        progressAnimation: 'increasing',
        timeOut: 10000,
        positionClass: 'toast-top-center'
      });
    this.GetBeneFactorDetailsStatistics();
    this.GetBeneFactorDetailsByBeneFactorId();
  }

  openCashSidePanel(content: any, item: any) {
    this.Type = item.name;
    this.TotalValue = item.totalValue;
    this.BeneFactorValueId = item.id;
    this.GetAllBeneFactorDetailsByValueId();
    this.offcanvasService.open(content, { position: 'end' });
  }

  GetBeneFactorDetailsStatistics() {
    this.adminService.GetBeneFactorDetailsStatistics(this.BeneFactorId).subscribe(data => {
      this.BeneFactorStatistics = data[0];
    });
  }

  GetBeneFactorDetailsByBeneFactorId() {
    this.adminService.GetBeneFactorDetailsByBeneFactorId(this.BeneFactorId, this.BeneFactorTypeId).subscribe(data => {
      this.BeneFactorDetailsData = data;
      if (this.FirstLoad) {
        this.BeneFactorTypeIds = [...new Set(this.BeneFactorDetailsData.map(i => i.beneFactorTypeId))];
        this.GetBeneFactorTypeByIds();
      }
      this.FirstLoad = false;
    });
  }

  GetBeneFactorTypeByIds() {
    this.adminService.GetBeneFactorTypeByIds(this.BeneFactorTypeIds).subscribe(data => {
      this.BeneFactorTypes = data;
    });
  }

  GetAllBeneFactorDetailsByValueId() {
    this.adminService.GetAllBeneFactorCashDetails(this.BeneFactorId, this.BeneFactorValueId).subscribe(data => {
      this.BeneFactorValues = data;
    });
  }

  OnChangeBeneFactorType(item: any) {
    this.BeneFactorTypeName = item.name;
    this.BeneFactorTypeId = item.id;
    this.GetBeneFactorDetailsByBeneFactorId();
  }

  LogOut() {
    localStorage.removeItem('BeneFactorModel');
    this.router.navigateByUrl('/');
  }

  OpenImageModal(content: any, src: any) {
    this.ImageSrc = src;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  OpenNoteModal(content: any) {
    this.Notes = '';
    this.Suggestions = '';
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  AddNewBeneFactorNotes() {
    if (!this.Notes && !this.Suggestions) {
      this.toaster.warning('برجاء إضافة ملاحظة او اقتراح');
      return;
    }

    let obj = {
      beneFactorId: this.BeneFactorId,
      note: this.Notes,
      suggestion: this.Suggestions
    }
    
    this.showLoader = true;
    this.adminService.AddNewBeneFactorNotes(obj).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    })
  }

}
