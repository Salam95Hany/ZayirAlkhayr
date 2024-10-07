import { DatePipe } from '@angular/common';
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FamilyNeeds } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { FamilyCategories, FamilyNeedTypes } from 'src/app/Admin/Models/GeneralStatus/FamilyStatusLookups';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-family-need',
  templateUrl: './family-need.component.html',
  styleUrls: ['./family-need.component.css']
})
export class FamilyNeedComponent implements OnInit, OnChanges {
  @Input() FamilyNeeds: FamilyNeedTypes[] = [];
  @Input() FamilyCategories: FamilyCategories[] = [];
  @Input() SelectedNeeds: FamilyNeeds[] = [];
  @Input() UpdateMode = false;
  @Input() DetailsMode = false;
  FamilyNeedsByCategory: FamilyNeedTypes[] = [];
  ItemForm: FormGroup;
  CategoryName = 'الفئات';
  FamilyNeedName = 'الاحتياجات';
  CategoryNameValidation = false;
  FamilyNeedNameValidation = false;
  FamilyNeedId: any;
  CategoryId: any;
  SelectedNeedId: any;
  addMode = true;
  isWaiting = true;

  constructor(private modalService: NgbModal, private fb: FormBuilder, private formService: ValidationFormService,
    private toaster: ToastrService, private datePipe: DatePipe
  ) { }

  ngOnInit(): void {
    this.FormInit();
    if ((this.UpdateMode && this.SelectedNeeds.length > 0) || this.DetailsMode)
      this.mergeFamilyNeedUpdateMode();
  }

  ngOnChanges(): void {
    if (this.UpdateMode && this.SelectedNeeds.length > 0)
      this.mergeFamilyNeedUpdateMode();
  }

  mergeFamilyNeedUpdateMode() {
    let category = '';
    this.SelectedNeeds.forEach(item => {
      let need = this.FamilyNeeds.find(i => i.id == item.needTypeId);
      if (need)
        category = this.FamilyCategories.find(i => i.id == need.categoryId)?.name;
      if (need && category) {
        item.categoryName = category;
        item.name = need.name;
      }
     if(item.deliveryDate)
      item.deliveryDate = this.datePipe.transform(item.deliveryDate,'yyyy-MM')
    });
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      deliveryDate: ['', [Validators.required]],
      isWaiting: true,
    });
  }

  FillEditForm(item: any) {
    this.FamilyNeedId = item.needTypeId;
    this.CategoryId = item.categoryId;
    this.CategoryName = item.categoryName;
    this.FamilyNeedName = item.name;
    this.isWaiting = item.isWaiting;
    this.FamilyNeedsByCategory = this.FamilyNeeds.filter(i => i.categoryId == this.CategoryId);
    this.ItemForm.setValue({
      id: item.id,
      deliveryDate: item?.deliveryDate,
      isWaiting: item.isWaiting,
    });
    this.updateDeliveryDateControlState();
  }

  ResetForm() {
    this.ItemForm.reset();
    this.CategoryName = 'الفئات';
    this.FamilyNeedName = 'الاحتياجات';
    this.FamilyNeedId = '';
    this.CategoryId = '';
    this.CategoryNameValidation = false;
    this.FamilyNeedNameValidation = false;
    this.isWaiting = true;
    this.updateDeliveryDateControlState();
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('isWaiting').setValue(true);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    this.addMode = true;
    if (item) {
      this.addMode = false;
      this.FillEditForm(item);
    }

    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, itemId: any) {
    this.SelectedNeedId = itemId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  updateDeliveryDateControlState(): void {
    if (this.isWaiting) {
      this.ItemForm.get('deliveryDate')?.disable();
    } else {
      this.ItemForm.get('deliveryDate')?.enable();
    }
  }

  onIsWaitingChange(newState: boolean): void {
    this.isWaiting = newState;
    this.updateDeliveryDateControlState();
  }

  OnChangeFamilyCategories(item: any) {
    this.CategoryId = item.id;
    this.CategoryName = item.name;
    this.CategoryNameValidation = false;
    this.FamilyNeedsByCategory = this.FamilyNeeds.filter(i => i.categoryId == this.CategoryId);
  }

  OnChangeFamilyNeedsByCategory(item: any) {
    this.FamilyNeedId = item.id;
    this.FamilyNeedName = item.name;
    this.FamilyNeedNameValidation = false;
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    this.CategoryNameValidation = this.CategoryName == 'الفئات';
    this.FamilyNeedNameValidation = this.FamilyNeedName == 'الاحتياجات';
    if (!isValid || this.CategoryNameValidation || this.FamilyNeedNameValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    if (this.addMode) {
      let checked = this.SelectedNeeds.find(i => i.needTypeId == this.FamilyNeedId);
      if (checked) {
        this.toaster.warning('هذا العنصر موجود');
        return;
      }
    }

    const formData = this.ItemForm.value;
    let arryNum = this.SelectedNeeds.map(i => i.id);
    let id = arryNum.length > 0 ? Math.max(...arryNum) : 0;
    if (this.addMode) {
      this.SelectedNeeds.push({
        id: id + 1,
        categoryId: this.CategoryId,
        needTypeId: this.FamilyNeedId,
        categoryName: this.CategoryName,
        name: this.FamilyNeedName,
        deliveryDate: formData.deliveryDate ?? null,
        isWaiting: formData.isWaiting
      });
    } else {
      let obj = this.SelectedNeeds.find(i => i.id == formData.id);
      if (obj) {
        obj.categoryId = this.CategoryId;
        obj.needTypeId = this.FamilyNeedId;
        obj.categoryName = this.CategoryName;
        obj.name = this.FamilyNeedName;
        obj.deliveryDate = formData.deliveryDate ?? null;
        obj.isWaiting = formData.isWaiting;
      }
    }

    this.modalService.dismissAll();
  }

  DeleteItem() {
    this.SelectedNeeds = this.SelectedNeeds.filter(i => i.id != this.SelectedNeedId);
    this.modalService.dismissAll();
  }

  GetOutputData() {
    return this.SelectedNeeds;
  }
}
