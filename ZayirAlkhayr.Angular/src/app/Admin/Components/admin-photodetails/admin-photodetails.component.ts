import { Component, TemplateRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-admin-photodetails',
  templateUrl: './admin-photodetails.component.html',
  styleUrls: ['./admin-photodetails.component.css']
})
export class AdminPhotodetailsComponent {
  isFilter = false;

  constructor(private modalService: NgbModal) { }

  openAddItemModal(content: TemplateRef<any>) {
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    })
  }

  openDeleteItemModal(content: TemplateRef<any>) {
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    })
  }
}
