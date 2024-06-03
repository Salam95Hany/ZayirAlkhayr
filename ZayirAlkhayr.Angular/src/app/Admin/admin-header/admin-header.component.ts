import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class AdminHeaderComponent {
  @Output() collapseExpandContent = new EventEmitter<boolean>();
  @Input() isCollapseOrExpand = false;
  isCollapseExpandContent = false;
  isSearchOpen = false;
  collapsed = true;

  onCollapseExpandMenu() {
    this.collapseExpandContent.emit();
  }
}
