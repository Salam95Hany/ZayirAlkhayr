import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class AdminHeaderComponent implements OnInit {
  @Output() collapseExpandContent = new EventEmitter<boolean>();
  @Input() isCollapseOrExpand = false;
  isCollapseExpandContent = false;
  collapsed = true;
  UserModel: any;
  constructor() {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  }
  onCollapseExpandMenu() {
    this.collapseExpandContent.emit();
  }
}
