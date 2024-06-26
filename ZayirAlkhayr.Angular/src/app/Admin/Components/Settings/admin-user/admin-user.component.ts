import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';

@Component({
  selector: 'app-admin-user',
  templateUrl: './admin-user.component.html',
  styleUrls: ['./admin-user.component.css']
})
export class AdminUserComponent implements OnInit {
  UsersData: any[] = [];
  ItemForm: FormGroup;
  UserId: any;
  RoleName = '--اختر--';
  Roles = ['SupperAdmin', 'WebSite', 'Services'];
  RoleValidation = false;
  ManagerUserId = '321db4e1-e32b-4aeb-8802-b076f9d7227d';
  TotalCount = 0;

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.FormInit();
    this.GetAllUsers();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      userId: null,
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      role: null
    });
  }

  FillEditForm(item: any) {
    this.RoleName = item?.role;
    this.UserId = item.userId;
    this.ItemForm.setValue({
      userId: item.userId,
      userName: item.userName,
      email: item.email,
      password: null,
      phoneNumber: item.phoneNumber,
      address: item?.address,
      role: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.RoleName = '--اختر--';
    this.RoleValidation = false;
    this.ItemForm.get('userId').setValue(0);
  }

  NumbersOnly(key: any): boolean {
    return this.formService.NumbersOnly(key);
  }

  openAddUserModal(content: any, item: any) {
    this.ResetForm();
    if (item)
      this.FillEditForm(item);
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteUserModal(content: any, userId: any) {
    this.UserId = userId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetAllUsers() {
    this.adminService.GetAllUsers().subscribe(data => {
      this.UsersData = data;
      this.TotalCount = this.UsersData.length;
    })
  }

  AddNewUser() {
    let isValid = this.ItemForm.valid;
    this.RoleValidation = this.RoleName.startsWith('--');
    if (!isValid || this.RoleValidation) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.ItemForm.patchValue({ role: this.RoleName });
    if (!this.ItemForm.controls['userId'].value) {
      this.adminService.CreateUser(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllUsers();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    } else {
      if (this.ManagerUserId == this.UserId) {
        this.toaster.warning('لا يمكن التعديل على هذا المستخدم');
        return;
      }
      this.adminService.EditUser(this.ItemForm.value).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetAllUsers();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    }
  }

  DeleteUser() {
    if (this.ManagerUserId == this.UserId) {
      this.toaster.warning('لا يمكن حذف هذا المستخدم');
      return;
    }
    this.adminService.DeleteUser(this.UserId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllUsers();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
    });
  }

}
