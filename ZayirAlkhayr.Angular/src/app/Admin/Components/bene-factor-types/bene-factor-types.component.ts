import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-bene-factor-types',
  templateUrl: './bene-factor-types.component.html',
  styleUrls: ['./bene-factor-types.component.css']
})
export class BeneFactorTypesComponent implements OnInit {

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

  

}
