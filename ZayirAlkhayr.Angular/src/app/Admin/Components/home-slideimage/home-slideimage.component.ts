import { Component, TemplateRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home-slideimage',
  templateUrl: './home-slideimage.component.html',
  styleUrls: ['./home-slideimage.component.css']
})
export class HomeSlideimageComponent {
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
