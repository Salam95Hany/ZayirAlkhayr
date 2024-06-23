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
  ItemForm: FormGroup;
  Roles = ['SupperAdmin', 'WebSite', 'Services'];

  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.FormInit();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      userName: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      role: ['', Validators.required]
    });
  }

  openAddUserModal(content: any, item: any) {
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

}
