import { Component, EventEmitter, forwardRef, Input, Output, Renderer2 } from '@angular/core';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-admin-drop-down',
  templateUrl: './admin-drop-down.component.html',
  styleUrls: ['./admin-drop-down.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AdminDropDownComponent),
      multi: true,
    },
  ],
})
export class AdminDropDownComponent {
  @Input() data: any[] = [];
  @Input() placeholder: string = '';
  @Input() colSize: string = 'col-lg-6';
  @Input() disabled: boolean = false;
  @Input() error: any;
  @Output() valueChanged = new EventEmitter<any | any[]>();
  searchText: string = '';
  selectedValue: any = '';
  selectedName: string = '';
  searchFields: string[] = ['name'];

  private onChange: any = () => { };
  private onTouched: any = () => { };
  constructor(private dropdownConfig: NgbDropdownConfig, private renderer: Renderer2) { }

  ngOnInit(): void {
    this.dropdownConfig.container = null;
  }

  ngOnChanges(changes: any): void {
    if (changes.data) {
      this.writeValue(this.selectedValue);
    }
  }

  writeValue(value: any): void {
    var option = this.data?.find(x => x.id === value);

    if (value)
      this.selectedValue = value;
    if (option)
      this.selectedName = option.name;
    else
      this.selectedName = '';
  }


  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInputChange(event: any) {
    const inputValue = event.target.value.toLowerCase();
    this.searchText = inputValue;
  }

  selectOption(option: any): void {
    this.selectedValue = option.id;
    this.onChange(this.selectedValue);
    this.onTouched();
    this.selectedName = option.name;
    this.valueChanged.emit(option.id);
  }
}
