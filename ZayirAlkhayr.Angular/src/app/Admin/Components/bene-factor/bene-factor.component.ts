import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-bene-factor',
  templateUrl: './bene-factor.component.html',
  styleUrls: ['./bene-factor.component.css']
})
export class BeneFactorComponent implements OnInit {
  isFilter = false;

  constructor(private modalService: NgbModal, private offcanvasService: NgbOffcanvas) {

  }

  ngOnInit(): void {

  }

  openAddItemModal(content: any, item: any) {
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  openCashSidePanel(content: any) {
    this.offcanvasService.open(content, { position: 'end' });
  }

}
