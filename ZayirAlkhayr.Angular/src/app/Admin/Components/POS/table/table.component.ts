import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements OnInit {
  Results: any[] = [];
  UserModel: any;
  showLoader = false;
  ItemForm: FormGroup;
  Total = 0;
  TableId: any;
  formErrors = {
    tableName: ''
  };

  constructor(private modalService: NgbModal, private adminService: AdminService,
    private formService: ValidationFormService,
    private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.FormInit();
    this.GetAllTable();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      tableId: 0,
      tableName: ['', [Validators.required, this.formService.noSpaceValidator]],
      insertUser: null,
    });

    this.ItemForm.valueChanges.subscribe((data) => {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, true);
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.setValue({
      tableId: item.tableId,
      tableName: item.tableName,
      insertUser: this.UserModel?.userId,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('tableId').setValue(0);
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
    this.TableId = item.tableId
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllTable() {
    this.showLoader = true;
    this.adminService.GetAllTable().subscribe(data => {
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
    if (this.ItemForm.controls['tableId'].value == 0) {
      this.adminService.AddNewTable(this.ItemForm.value).subscribe(data => {
        this.showLoader = false;
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllTable();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
        this.showLoader = false;
      });
    } else {
      this.adminService.UpdateTable(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.toaster.success(data.message);
          this.GetAllTable();
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
    this.adminService.DeleteTable(this.TableId).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetAllTable();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
