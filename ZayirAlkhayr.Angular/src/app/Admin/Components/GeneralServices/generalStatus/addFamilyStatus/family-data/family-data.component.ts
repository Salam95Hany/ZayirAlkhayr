import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FamilyDetails } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-family-data',
  templateUrl: './family-data.component.html',
  styleUrls: ['./family-data.component.css']
})
export class FamilyDataComponent implements OnInit {
  FamilyDetails: FamilyDetails[] = [];
  ItemForm: FormGroup;
  FamilyDetailsId: any;
  MaritalStatusName = 'الحالة الاجتماعية';
  MaritalStatusValidation = false;
  addMode = true;
  MaritalStatus: any[] = [
    { id: 1, name: 'أعزب' },
    { id: 1, name: 'متزوج' },
    { id: 1, name: 'مطلقة' },
    { id: 1, name: 'أرمل' }
  ];

  constructor(private modalService: NgbModal, private fb: FormBuilder, private formService: ValidationFormService) { }

  ngOnInit(): void {
    this.FormInit();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      relevance: ['', [Validators.required, this.formService.noSpaceValidator]],
      age: ['', [Validators.required, this.formService.noSpaceValidator, Validators.pattern("[0-9]+")]],
      maritalStatus: null,
      education: ['', [Validators.required, this.formService.noSpaceValidator]],
      jop: ['', [Validators.required, this.formService.noSpaceValidator]],
      nationalId: ['', [Validators.required, this.formService.noSpaceValidator, Validators.pattern("[0-9]+")]],
    });
  }

  FillEditForm(item: any) {
    this.MaritalStatusName = item.maritalStatus;
    this.ItemForm.setValue({
      id: item.id,
      name: item.name,
      relevance: item.relevance,
      age: item.age,
      maritalStatus: item.maritalStatus,
      education: item.education,
      jop: item.jop,
      nationalId: item.nationalId,
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

    this.ItemForm.patchValue({ maritalStatus: this.MaritalStatusName });
    const formData = this.ItemForm.value;
    let arryNum = this.FamilyDetails.map(i => i.id);
    let id = arryNum.length > 0 ? Math.max(...arryNum) : 0;
    if (this.addMode) {
      this.FamilyDetails.push({
        id: id + 1,
        name: formData.name,
        relevance: formData.relevance,
        age: formData.age,
        maritalStatus: formData.maritalStatus,
        education: formData.education,
        jop: formData.jop,
        nationalId: formData.nationalId
      });
    } else {
      let obj = this.FamilyDetails.find(i => i.id == formData.id);
      if (obj) {
        obj.name = formData.name;
        obj.relevance = formData.relevance;
        obj.age = formData.age;
        obj.maritalStatus = formData.maritalStatus;
        obj.education = formData.education;
        obj.jop = formData.jop;
        obj.nationalId = formData.nationalId;
      }
    }

    this.modalService.dismissAll();
  }

  DeleteItem() {
    this.FamilyDetails = this.FamilyDetails.filter(i => i.id != this.FamilyDetailsId);
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
      return {};
  }

}
