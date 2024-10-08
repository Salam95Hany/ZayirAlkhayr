import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FamilyDetails, FamilyPatient } from 'src/app/Admin/Models/GeneralStatus/AddFamilyStatusModel';
import { FamilyPatientTypes } from 'src/app/Admin/Models/GeneralStatus/FamilyStatusLookups';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-family-medical',
  templateUrl: './family-medical.component.html',
  styleUrls: ['./family-medical.component.css']
})
export class FamilyMedicalComponent implements OnInit, OnChanges {
  @Input() PatientTypes: FamilyPatientTypes[] = [];
  @Input() FamilyDetails: FamilyDetails[] = [];
  @Input() FamilyStatusName: string;
  @Input() UpdateMode = false;
  @Input() DetailsMode = false;
  @Input() FamilyPatients: FamilyPatient[] = [];
  FamilyNames: string[] = [];
  ItemForm: FormGroup;
  FamilyName = 'اسم الفرد';
  PatientTypeName = 'التخصص';
  FamilyNameValidation = false;
  PatientTypeNameValidation = false;
  PatientTypeId: any;
  FamilyPatientId: any;
  addMode = true;

  constructor(private modalService: NgbModal, private fb: FormBuilder, private formService: ValidationFormService,
    private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.FormInit();
    if (this.UpdateMode && this.FamilyPatients.length > 0) {
      this.InetialData();
      this.mergeFamilyPatientUpdateMode();
    }

    if (this.DetailsMode)
      this.mergeFamilyPatientUpdateMode();
  }

  ngOnChanges(changes: SimpleChanges): void {
    let name = changes['FamilyStatusName'];
    let familyDetails = changes['FamilyDetails']
    if (this.UpdateMode) {
      if (name) {
        if (name.previousValue) {
          let obj = this.FamilyPatients.find(i => i.name == name.previousValue);
          if (obj)
            obj.name = name.currentValue;
        }
        this.InetialData();
      }

      if (familyDetails) {
        this.InetialData();
      }
    }
  }

  mergeFamilyPatientUpdateMode() {
    this.FamilyPatients.forEach(item => {
      let obj = this.PatientTypes.find(i => i.id == item.patientTypeId);
      if (obj)
        item.patientTypeName = obj.name;
    });
  }

  InetialData() {
    if (this.FamilyDetails.length == 0) {
      const objectToKeep = this.FamilyPatients.find(i => i.name == this.FamilyStatusName);
      this.FamilyPatients = objectToKeep ? [objectToKeep] : [];
    } else {
      this.FamilyDetails.forEach(item => {
        let obj = this.FamilyPatients.find(i => i.name == item.oldName);
        if (obj)
          obj.name = item.name;
      });
      this.FamilyPatients.forEach((item, index) => {
        let obj = this.FamilyDetails.find(i => i.name == item.name);
        if (!obj)
          this.FamilyPatients.splice(index, 1);
      });
    }

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
    this.PatientTypeName = 'التخصص';
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
    this.PatientTypeNameValidation = this.PatientTypeName == 'التخصص';
    if (!isValid || this.FamilyNameValidation || this.PatientTypeNameValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }

    if (this.addMode) {
      let checked = this.FamilyPatients.find(i => i.name == this.FamilyName);
      if (checked) {
        this.toaster.warning('هذا العنصر موجود');
        return;
      }
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
    return this.FamilyPatients;
  }
}
