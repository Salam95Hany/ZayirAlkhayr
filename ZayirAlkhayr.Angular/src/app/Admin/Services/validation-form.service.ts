import { Injectable } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidationFormService {
  fileURL: any[] = [];
  UserModel: any;
  constructor() {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsTouched({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }

  buildFormData(formData, data, parentKey = null, key = null) {
    if (data instanceof File)
      formData.append(key, data);
    if (data && typeof data === 'object') {
      Object.keys(data).forEach(key => {
        this.buildFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key, key);
      });
    } else {
      const value = data == null ? '' : data;

      formData.append(parentKey, value);
    }
  }

  onSelectedFile(file: any): Promise<any[]> {
    if (file) {
      const promises = [];
      for (let x = 0; x < file.length; x++) {
        const reader = new FileReader();
        const URL = new Promise<string>((resolve, reject) => {
          reader.onload = (events: any) => {
            resolve(events.target.result as string);
          };
          reader.readAsDataURL(file[x]);
        });
        const fileContent = new Promise<string>((resolve, reject) => {
          resolve(file);
        });
        promises.push(URL);
        promises.push(fileContent);
      }
      return Promise.all(promises);
    }
    return Promise.resolve([]);
  }

  NumbersOnly(key: any): boolean {
    let patt = /^([0-9\+])$/;
    let result = patt.test(key);
    return result;
  }
}
