import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ValidationFormService } from '../Services/validation-form.service';

@Component({
  selector: 'app-admin-side-menu',
  templateUrl: './admin-side-menu.component.html',
  styleUrls: ['./admin-side-menu.component.css']
})
export class AdminSideMenuComponent implements OnInit {
  @Input() isCollapsing = false;
  @Output() closeSideMenuFromOverlayEvent = new EventEmitter<boolean>();
  isCollapsed_1 = false;
  UserModel: any;

  constructor(private formService: ValidationFormService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  }

  onCloseSidemenuFromOverlay() {
    this.closeSideMenuFromOverlayEvent.emit();
  }
}
