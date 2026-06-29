import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from 'src/app/Admin/Services/employee.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-jop-title',
  templateUrl: './jop-title.component.html',
  styleUrls: ['./jop-title.component.css']
})
export class JopTitleComponent implements OnInit {
  Results: any[] = [];
  UserModel: any;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  JobTitleId: any;
  formErrors = {
    name: ''
  };

  constructor(private modalService: NgbModal, private employeeService: EmployeeService,
    private formService: ValidationFormService,
    private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllJobTitle();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      jobTitleId: 0,
      name: ['', [Validators.required, this.formService.noSpaceValidator]],
      description: [''],
      insertUser: null,
    });

    this.ItemForm.valueChanges.subscribe((data) => {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, true);
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      jobTitleId: item.jobTitleId,
      name: item.name,
      description: item.description,
      insertUser: this.UserModel?.userId,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('jobTitleId').setValue(0);
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item)
      this.FillEditForm(item);

    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, item: any) {
    this.JobTitleId = item.jobTitleId
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllJobTitle() {
    this.showLoader = true;
    this.employeeService.GetAllJobTitle().subscribe(data => {
      this.showLoader = false;
      this.Results = data.results;
      this.Total = data.totalCount;
    });
  }

  validateForm(): boolean {
    this.formService.markFormGroupTouched(this.ItemForm);
    if (this.ItemForm.valid) {
      return true;
    } else {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, false)
      return false;
    }
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.validateForm();
    if (!isValid)
      return;

    this.showLoader = true;
    if (this.ItemForm.controls['jobTitleId'].value == 0) {
      this.employeeService.AddNewJobTitle(this.ItemForm.value).subscribe(data => {
        this.showLoader = false;
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllJobTitle();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.employeeService.UpdateJobTitle(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllJobTitle();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    }
  }

  DeleteItem() {
    this.showLoader = true;
    this.employeeService.DeleteJobTitle(this.JobTitleId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllJobTitle();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
