import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, EventEmitter, forwardRef, Input, OnChanges, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
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
export class AdminDropDownComponent implements OnInit, OnChanges, AfterViewInit {
  @ViewChild('dropdownButton') dropdownButton!: ElementRef<HTMLButtonElement>;
  @Input() data: any[] = [];
  @Input() placeholder: string = '';
  @Input() disabled: boolean = false;
  @Input() error: any;
  @Output() valueChanged = new EventEmitter<any | any[]>();
  searchText: string = '';
  selectedValue: any = '';
  selectedName: string = '';
  dropdownWidth = 0;
  searchFields: string[] = ['name'];

  private onChange: any = () => { };
  private onTouched: any = () => { };
  constructor(private dropdownConfig: NgbDropdownConfig, private renderer: Renderer2,private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.dropdownConfig.container = null;
  }

  ngAfterViewInit() {
    this.dropdownWidth = this.dropdownButton.nativeElement.offsetWidth;
    this.cdr.detectChanges();
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
