import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FamilyDetails } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-family-data',
  templateUrl: './family-data.component.html',
  styleUrls: ['./family-data.component.css']
})
export class FamilyDataComponent implements OnInit {
  @Output() FamilyDetailsChange = new EventEmitter<FamilyDetails[]>();
  @Input() FamilyDetails: FamilyDetails[] = [];
  @Input() UpdateMode = false;
  @Input() DetailsMode = false;
  ItemForm: FormGroup;
  FamilyDetailsId: any;
  MaritalStatusName = 'الحالة الاجتماعية';
  FamilyChild = ["ابنة", "إبنة", "ابنه", "ابن", "إبنه", "إبن"];
  MaritalStatusValidation = false;
  FamilyChildCount = 0;
  addMode = true;
  MaritalStatus: any[] = [
    { id: 1, name: 'أعزب' },
    { id: 1, name: 'متزوج' },
    { id: 1, name: 'مطلقة' },
    { id: 1, name: 'أرمل' }
  ];

  constructor(private modalService: NgbModal, private fb: FormBuilder, private formService: ValidationFormService,
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.FormInit();
    this.FamilyChildCount = this.FamilyDetails.filter(i => this.FamilyChild.includes(i.relevance)).length;
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      relevance: ['', [Validators.required, this.formService.noSpaceValidator]],
      age: ['', [Validators.required, this.formService.noSpaceValidator, Validators.pattern("[0-9]+")]],
      education: ['', [Validators.required, this.formService.noSpaceValidator]],
      jop: ['', [Validators.required, this.formService.noSpaceValidator]],
      nationalId: ['', [this.formService.noSpaceValidator]],
    });
  }

  FillEditForm(item: any) {
    this.MaritalStatusName = item.maritalStatus;
    this.ItemForm.setValue({
      id: item.id,
      name: item.name,
      relevance: item.relevance,
      age: item.age,
      education: item.education,
      jop: item.jop,
      nationalId: item?.nationalId,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.MaritalStatusName = 'الحالة الاجتماعية';
    this.MaritalStatusValidation = false;
    this.ItemForm.get('id').setValue(0);
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
    this.FamilyDetailsId = itemId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  OnChangeMaritalStatus(item: any) {
    this.MaritalStatusName = item.name;
    this.MaritalStatusValidation = false;
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    this.MaritalStatusValidation = this.MaritalStatusName == 'الحالة الاجتماعية';
    if (!isValid || this.MaritalStatusValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    if (this.addMode) {
      let checked = this.FamilyDetails.find(i => i.name == this.ItemForm.value.name);
      if (checked) {
        this.toaster.warning('هذا العنصر موجود');
        return;
      }
    }

    const formData = this.ItemForm.value;
    let arryNum = this.FamilyDetails.map(i => i.id);
    let id = arryNum.length > 0 ? Math.max(...arryNum) : 0;
    if (this.addMode) {
      this.FamilyDetails.push({
        id: id + 1,
        oldName: formData.name,
        name: formData.name,
        relevance: formData.relevance,
        age: formData.age,
        maritalStatus: this.MaritalStatusName,
        education: formData.education,
        jop: formData.jop,
        nationalId: formData.nationalId
      });
    } else {
      let obj = this.FamilyDetails.find(i => i.id == formData.id);
      if (obj) {
        obj.oldName = obj.name;
        obj.name = formData.name;
        obj.relevance = formData.relevance;
        obj.age = formData.age;
        obj.maritalStatus = this.MaritalStatusName;
        obj.education = formData.education;
        obj.jop = formData.jop;
        obj.nationalId = formData.nationalId;
      }
    }

    this.FamilyChildCount = this.FamilyDetails.filter(i => this.FamilyChild.includes(i.relevance)).length;
    if (this.UpdateMode)
      this.FamilyDetailsChange.emit(this.FamilyDetails);
    this.modalService.dismissAll();
  }

  DeleteItem() {
    this.FamilyDetails = this.FamilyDetails.filter(i => i.id != this.FamilyDetailsId);
    if (this.UpdateMode)
      this.FamilyDetailsChange.emit(this.FamilyDetails);
    this.FamilyChildCount = this.FamilyDetails.filter(i => this.FamilyChild.includes(i.relevance)).length;
    this.modalService.dismissAll();
  }

  GetOutputData() {
    if (this.FamilyDetails.length > 0) {
      this.FamilyDetails.forEach(item => {
        item.childernsCount = this.FamilyDetails.length;
        item.familyMembersCount = this.FamilyDetails.length + 1;
      });

      return this.FamilyDetails;
    } else
      return [];
  }

}
