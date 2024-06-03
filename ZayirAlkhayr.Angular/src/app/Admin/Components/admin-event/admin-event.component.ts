import { Component, TemplateRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-admin-event',
  templateUrl: './admin-event.component.html',
  styleUrls: ['./admin-event.component.css']
})
export class AdminEventComponent {
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
