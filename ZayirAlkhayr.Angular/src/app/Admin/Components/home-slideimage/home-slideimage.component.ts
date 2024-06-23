import { AfterViewInit, Component, ElementRef, OnInit, TemplateRef, ViewChild, ViewChildren } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminWebsiteService } from '../../Services/admin-website.service';
import { ValidationFormService } from '../../Services/validation-form.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home-slideimage',
  templateUrl: './home-slideimage.component.html',
  styleUrls: ['./home-slideimage.component.css']
})
export class HomeSlideimageComponent implements OnInit {
  @ViewChild('InputFile') InputFile: ElementRef;
  isFilter = false;
  SliderData: any[] = [];
  ItemForm: FormGroup;
  isFileExist = false;
  ImageFile: any;
  UserModel: any;
  TotalCount = 0;
  fileURL: any[] = [];
  SliderId: number;
  constructor(private modalService: NgbModal, private adminService: AdminWebsiteService, private formService: ValidationFormService
    , private fb: FormBuilder, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = this.formService.UserModel;
    this.FormInit();
    this.GetHomeSliderImages();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      id: 0,
      title: ['', Validators.required],
      isVisible: true,
      InsertUser: null,
      oldFileName: null,
      file: null,
    });
  }

  FillEditForm(item: any) {
    this.fileURL = [];
    this.fileURL.push(item);
    let fileName = item.image.split('\\');
    this.ItemForm.setValue({
      id: item.id,
      title: item?.title,
      isVisible: item?.isVisible,
      oldFileName: fileName[fileName.length - 1],
      InsertUser: this.UserModel?.userId,
      file: null,
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('id').setValue(0);
    this.ItemForm.get('isVisible').setValue(true);
    this.ItemForm.get('InsertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    this.isFileExist = false;
    this.fileURL = [];
    this.ImageFile = null;
    if (item)
      this.FillEditForm(item);

    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    })
  }

  openDeleteItemModal(content: any, item: any) {
    this.SliderId = item.id;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    })
  }

  GetHomeSliderImages() {
    this.adminService.GetHomeSliderImages().subscribe(data => {
      this.SliderData = data;
      this.SliderData.forEach(i => { i.isVisible = i.isVisible == 'True' ? true : false });
      this.TotalCount = data.length;
    });
  }

  onFileChange(event: any) {
    this.fileURL = [];
    this.ImageFile = null;
    this.formService.onSelectedFile(event.target.files).then(data => {
      this.fileURL.push(data[0]);
      this.ImageFile = data[1][0];
      this.isFileExist = false;
    });
  }

  DeleteSelectedFile() {
    this.ImageFile = null;
    this.fileURL = [];
    this.InputFile.nativeElement.value = '';
  }

  AddNewSlider() {
    let isValid = this.ItemForm.valid;
    this.isFileExist = this.fileURL.length == 0;
    if (this.isFileExist)
      return;

    if (!isValid) {
      this.formService.validateAllFormFields(this.ItemForm);
      return;
    }
    this.ItemForm.patchValue({ file: this.ImageFile });
    const formData = new FormData();
    this.formService.buildFormData(formData, this.ItemForm.value);
    if (this.ItemForm.controls['id'].value == 0) {
      this.adminService.AddNewSliderImage(formData).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetHomeSliderImages();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    } else {
      this.adminService.UpdateSliderImage(formData).subscribe(data => {
        if (data.done) {
          this.toaster.success(data.message);
          this.GetHomeSliderImages();
          this.modalService.dismissAll();
        }
        else
          this.toaster.error(data.message);
      });
    }
  }

  DeleteSlider() {
    this.adminService.DeleteSliderImage(this.SliderId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetHomeSliderImages();
        this.modalService.dismissAll();
      }
      else
        this.toaster.error(data.message);
    });
  }
}
