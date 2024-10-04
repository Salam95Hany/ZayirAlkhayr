import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FamilyDetails, FamilyPatient } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { FamilyPatientTypes } from 'src/app/Admin/Models/GeneralStatus/FamilyStatusLookups';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-family-medical',
  templateUrl: './family-medical.component.html',
  styleUrls: ['./family-medical.component.css']
})
export class FamilyMedicalComponent {
  @Input() PatientTypes: FamilyPatientTypes[] = [];
  @Input() FamilyDetails: FamilyDetails[] = [];
  @Input() FamilyStatusName: string;
  FamilyPatients: FamilyPatient[] = [];
  FamilyNames: string[] = [];
  ItemForm: FormGroup;
  FamilyName = 'اسم الفرد';
  PatientTypeName = 'نوع المرض';
  FamilyNameValidation = false;
  PatientTypeNameValidation = false;
  PatientTypeId: any;
  FamilyPatientId: any;
  addMode = true;

  constructor(private modalService: NgbModal, private fb: FormBuilder, private formService: ValidationFormService) { }

  ngOnInit(): void {
    this.FormInit();
  }

  InetialData() {
    this.FamilyNames = [];
    this.FamilyNames.push(this.FamilyStatusName);
    if (this.FamilyDetails && this.FamilyDetails.length > 0)
      this.FamilyNames.push(...this.FamilyDetails.map(i => i.name));
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      patientDate: ['', [Validators.required]],
      specialization: ['', [Validators.required, this.formService.noSpaceValidator]],
      isMedicalReport: false,
      isNeedProcess: false,
    });
  }

  FillEditForm(item: any) {
    this.FamilyName = item.name;
    this.PatientTypeName = item.patientTypeName;
    this.PatientTypeId = item.patientTypeId;
    this.ItemForm.setValue({
      id: item.id,
      patientDate: item.patientDate,
      specialization: item.specialization,
      isMedicalReport: item.isMedicalReport,
      isNeedProcess: item.isNeedProcess
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.FamilyName = 'اسم الفرد';
    this.PatientTypeName = 'نوع المرض';
    this.PatientTypeId = '';
    this.FamilyNameValidation = false;
    this.PatientTypeNameValidation = false;
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
    this.FamilyPatientId = itemId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  OnChangeFamilyName(item: any) {
    this.FamilyName = item;
    this.FamilyNameValidation = false;
  }

  OnChangePatientTypesName(item: any) {
    this.PatientTypeId = item.id;
    this.PatientTypeName = item.name;
    this.PatientTypeNameValidation = false;
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;

    this.FamilyNameValidation = this.FamilyName == 'اسم الفرد';
    this.PatientTypeNameValidation = this.PatientTypeName == 'نوع المرض';
    if (!isValid || this.FamilyNameValidation || this.PatientTypeNameValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    const formData = this.ItemForm.value;
    let arryNum = this.FamilyPatients.map(i => i.id);
    let id = arryNum.length > 0 ? Math.max(...arryNum) : 0;
    if (this.addMode) {
      this.FamilyPatients.push({
        id: id + 1,
        name: this.FamilyName,
        patientTypeId: this.PatientTypeId,
        patientTypeName: this.PatientTypeName,
        patientDate: formData.patientDate,
        specialization: formData.specialization,
        isMedicalReport: formData.isMedicalReport,
        isNeedProcess: formData.isNeedProcess
      });
    } else {
      let obj = this.FamilyPatients.find(i => i.id == formData.id);
      if (obj) {
        obj.name = this.FamilyName;
        obj.patientTypeId = this.PatientTypeId;
        obj.patientTypeName = this.PatientTypeName;
        obj.patientDate = formData.patientDate;
        obj.specialization = formData.specialization;
        obj.isMedicalReport = formData.isMedicalReport;
        obj.isNeedProcess = formData.isNeedProcess;
      }
    }

    this.modalService.dismissAll();
  }

  DeleteItem() {
    this.FamilyPatients = this.FamilyPatients.filter(i => i.id != this.FamilyPatientId);
    this.modalService.dismissAll();
  }

  GetOutputData() {
    if (this.FamilyPatients.length > 0)
      return this.FamilyDetails;
    else
      return {};
  }
}
