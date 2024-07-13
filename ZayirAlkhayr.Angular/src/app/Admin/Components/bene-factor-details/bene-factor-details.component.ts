import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-bene-factor-details',
  templateUrl: './bene-factor-details.component.html',
  styleUrls: ['./bene-factor-details.component.css']
})
export class BeneFactorDetailsComponent implements OnInit {
  BenefactorType = 'All';
  constructor(private modalService: NgbModal) {

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

  GetBenefactorType(isSelected: boolean) {
    this.BenefactorType = isSelected ? 'Cash' : 'All';
  }

}
