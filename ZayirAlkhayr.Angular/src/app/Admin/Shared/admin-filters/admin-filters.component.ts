import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FilterModel } from '../../Models/FilterModel';

@Component({
  selector: 'app-admin-filters',
  templateUrl: './admin-filters.component.html',
  styleUrls: ['./admin-filters.component.css']
})
export class AdminFiltersComponent {
  @Output() FilterChecked = new EventEmitter<FilterModel[]>();
  @Input() FilterList: FilterModel[] = [];
  @Input() PlaceHolder: any;
  @Input() ApplyDateFilter = false;
  SelectedFilter: FilterModel[] = [];
  isFilterOnly = false;
  FilterSearchText = '';
  SearchText = '';
  DateFilter: any;

  constructor() { }

  InputSearchChange() {
    this.SelectedFilter = this.SelectedFilter.filter(i => i.categoryName != 'SearchText');
    if (this.SearchText)
      this.SelectedFilter.push({
        categoryName: 'SearchText',
        categoryNameAr: 'مربع البحث',
        itemId: this.SearchText,
        itemKey: this.SearchText
      });
    this.FilterChecked.emit(this.SelectedFilter);
  }

  filterChecked() {
    this.SelectedFilter = [];
    this.FilterList.map(item => {
      let checked = item.filterItems.filter(a => a.isChecked && a.isChecked == true);
      if (checked.length > 0) {
        checked.map(obj => {
          this.SelectedFilter.push(obj);
        });
      }
    });
    this.FilterChecked.emit(this.SelectedFilter);
  }

  RemoveSelectedFilter(filter: any, index: number) {
    this.SelectedFilter.splice(index, 1);
    this.FilterList.map(item => {
      let checked = item.filterItems.find(a => a.itemId == filter.itemId);
      if (checked) {
        checked.isChecked = false;
      }
    });
    if (filter.categoryName == 'SearchText')
      this.SearchText = '';
    if (filter.categoryName == 'Date')
      this.DateFilter = '';
    this.FilterChecked.emit(this.SelectedFilter);
  }

  RemoveAllFilters() {
    this.SelectedFilter = [];
    this.SearchText = '';
    this.DateFilter = '';
    this.FilterList.map(item => {
      item.filterItems.map(a => a.isChecked = false);
    });
    this.FilterChecked.emit(this.SelectedFilter);
  }

  DateFilterChange() {
    this.SelectedFilter = this.SelectedFilter.filter(i => i.categoryName != 'Date');
    if (this.DateFilter)
      this.SelectedFilter.push({
        categoryName: 'Date',
        categoryNameAr: 'التاريخ',
        itemId: this.DateFilter,
        itemKey: this.DateFilter
      });
    this.FilterChecked.emit(this.SelectedFilter);
  }
}
