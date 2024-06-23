import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ValidationFormService } from '../Services/validation-form.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class AdminHeaderComponent implements OnInit {
  @ViewChild("autoCompleteWrapper") autoCompleteWrapper: ElementRef;
  @Output() collapseExpandContent = new EventEmitter<boolean>();
  @Input() isCollapseOrExpand = false;
  isCollapseExpandContent = false;
  isSearchOpen = false;
  collapsed = true;
  showAutoCompleteMenu = false;
  UserModel: any;


  constructor(private formService: ValidationFormService, private router: Router) {
    this.onClickOutside;
  }

  ngOnInit(): void {
    this.UserModel = this.formService.UserModel;
  }

  onCollapseExpandMenu() {
    this.collapseExpandContent.emit();
  }

  onShowAutoCompleteMenu(input: HTMLInputElement) {
    this.showAutoCompleteMenu = true;
    if (input.value === '') {
      this.showAutoCompleteMenu = false;
    }
  }

  goToWebsite() {
    localStorage.removeItem('UserModel');
    this.router.navigateByUrl('/');
  }

  @HostListener('document:mousedown', ['$event']) onClickOutside(event: Event) {
    if (!this.autoCompleteWrapper.nativeElement.contains(event.target)) {
      this.showAutoCompleteMenu = false;
      this.isSearchOpen = false;
    } else {
      return;
    }
  }

}
