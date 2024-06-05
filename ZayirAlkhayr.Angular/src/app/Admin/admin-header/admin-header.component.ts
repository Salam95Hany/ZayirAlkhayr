import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from '@angular/core';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.css']
})
export class AdminHeaderComponent implements OnInit {
  @Output() collapseExpandContent = new EventEmitter<boolean>();
  @Input() isCollapseOrExpand = false;
  isCollapseExpandContent = false;
  isSearchOpen = false;
  collapsed = true;
  showAutoCompleteMenu = false;


  constructor() {
    this.onClickOutside;
  }

  @ViewChild("autoCompleteWrapper") autoCompleteWrapper: ElementRef;

  onCollapseExpandMenu() {
    this.collapseExpandContent.emit();
  }

  onShowAutoCompleteMenu(input: HTMLInputElement) {
    this.showAutoCompleteMenu = true;
    if (input.value === '') {
      this.showAutoCompleteMenu = false;
    }
  }

  ngOnInit(): void {
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
