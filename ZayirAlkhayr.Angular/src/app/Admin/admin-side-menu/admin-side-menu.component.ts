import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-admin-side-menu',
  templateUrl: './admin-side-menu.component.html',
  styleUrls: ['./admin-side-menu.component.css']
})
export class AdminSideMenuComponent {
  @Input() isCollapsing = false;
  @Output() closeSideMenuFromOverlayEvent = new EventEmitter<boolean>();
  isCollapsed_1 = false;

  onCloseSidemenuFromOverlay() {
    this.closeSideMenuFromOverlayEvent.emit();
  }
}
