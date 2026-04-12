import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/Admin/Services/admin.service';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';
import { AuthService } from 'src/app/Auth/auth.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  UserInfo: any;
  UserId: any;
  EditProfileForm: FormGroup;
  ChangePasswordForm: FormGroup;
  BtnDisabled = false;
  showLoader = false;
  UserModel: any;
  Roles = [
    { nameEn: 'Admin', nameAr: 'مدير' },
    { nameEn: 'Cashier', nameAr: 'كاشير' }
  ];
  EditProfileErrors = {
    email: '',
    phoneNumber: '',
    address: ''
  };
  ChangePasswordFormErrors = {
    password: ''
  };

  constructor(private adminService: AdminService, private formService: ValidationFormService, private fb: FormBuilder,
    private toaster: ToastrService, private modalService: NgbModal, private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.UserId = this.UserModel.userId;
    this.GetUserInfoById();
    this.EditProfileFormInit();
    this.ChangePasswordFormInit();
  }

  EditProfileFormInit() {
    this.EditProfileForm = this.fb.group({
      userId: null,
      email: ['', [Validators.required, Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]],
      phoneNumber: ['', [Validators.required]],
      address: ['', [Validators.required]],
    });
  }

  ChangePasswordFormInit() {
    this.ChangePasswordForm = this.fb.group({
      userId: null,
      password: ['', [Validators.required, Validators.pattern("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z0-9]).+$")]],
    });
  }

  FillEditForm() {
    this.EditProfileForm.patchValue({
      userId: this.UserInfo?.userId ?? '',
      email: this.UserInfo?.email ?? '',
      phoneNumber: this.UserInfo?.phoneNumber ?? '',
      address: this.UserInfo?.address ?? '',
    });
  }

  OpenEditProfileModal(content: any) {
    this.EditProfileForm.reset();
    this.FillEditForm();
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    })
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }

  OpenChangePasswordModal(content: any) {
    this.ChangePasswordForm.reset();
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    })
  }

  GetUserInfoById() {
    this.showLoader = true;
    this.adminService.GetUserInfoById(this.UserId).subscribe((res) => {
      this.showLoader = false;
      this.UserInfo = res.results;
      let role = this.Roles.find(i => i.nameEn == this.UserInfo.role);
      if (role)
        this.UserInfo.roleNameAr = role.nameAr;
    });
  }

  EditUserProfile() {
    this.EditProfileForm = this.formService.TrimFormInputValue(this.EditProfileForm);
    let isValid = this.EditProfileForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.EditProfileForm);
      return;
    }

    this.showLoader = true;
    this.EditProfileForm.patchValue({ userId: this.UserId });
    this.BtnDisabled = true;
    this.adminService.EditUserProfile(this.EditProfileForm.value).subscribe((res) => {
      this.showLoader = false;
      this.BtnDisabled = false;
      if (res.isSuccess) {
        this.toaster.success(res.message);
        this.GetUserInfoById();
        this.modalService.dismissAll();
      } else {
        this.toaster.error(res.message);
      }
    });
  }

  ChangeUserPassword() {
    this.ChangePasswordForm = this.formService.TrimFormInputValue(this.ChangePasswordForm);
    let isValid = this.ChangePasswordForm.valid;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ChangePasswordForm);
      return;
    }

    this.showLoader = true;
    this.ChangePasswordForm.patchValue({ userId: this.UserId });
    this.BtnDisabled = true;
    this.adminService.ChangeUserPassword(this.ChangePasswordForm.value).subscribe((res) => {
      this.showLoader = false;
      this.BtnDisabled = false;
      if (res.isSuccess) {
        this.toaster.success(res.message);
        this.modalService.dismissAll();
        this.authService.loginRedirect();
      } else {
        this.toaster.error(res.message);
      }
    });
  }
}
