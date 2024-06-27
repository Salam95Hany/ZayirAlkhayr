import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ValidationFormService } from '../Services/validation-form.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Auth/auth.service';

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


  constructor(private formService: ValidationFormService, private router: Router, private authService: AuthService) {
    this.onClickOutside;
  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));;
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
    this.authService.AdminLogout(this.UserModel?.userId).subscribe(data => {
      if (data) {
        localStorage.removeItem('UserModel');
        this.router.navigateByUrl('/');
      }
    });
  }

  @HostListener('document:mousedown', ['$event']) onClickOutside(event: Event) {
    if (!this.autoCompleteWrapper.nativeElement.contains(event.target)) {
      this.showAutoCompleteMenu = false;
      this.isSearchOpen = false;
    } else {
      return;
    }
  }

  Logout() {
    this.authService.AdminLogout(this.UserModel?.userId).subscribe(data => {
      if (data) {
        localStorage.removeItem('UserModel');
        this.router.navigateByUrl('/login');
      }
    });
  }

}
