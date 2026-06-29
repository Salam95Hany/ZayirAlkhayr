import { Injectable } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { v4 as uuidv4 } from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class ValidationFormService {
  fileURL: any[] = [];

  constructor() { }

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
      formData.append('Files', data);
    else if (data && typeof data === 'object' && !(data instanceof File)) {
      Object.keys(data).forEach(key => {
        this.buildFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key, key);
      });
    } else {
      const value = data == null ? '' : data;
      formData.append(parentKey, value);
    }
  }

  getFileSize(file: any): number {
    const fileSizeInKB = file.size / 1024;
    const fileSizeInMB = Math.round(fileSizeInKB / 1024);
    return fileSizeInMB;
  }

  onSelectedFile(file: any): Promise<any[]> {
    if (file) {
      const promises = [];
      for (let x = 0; x < file.length; x++) {
        const reader = new FileReader();
        const URL = new Promise<any>((resolve, reject) => {
          reader.onload = (events: any) => {
            resolve({ image: events.target.result });
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

  onSelectedMultiFile(files: any): Promise<{ urls: any[]; fileContents: any[] }> {
    const urls: any[] = [];
    const fileContents: any[] = [];
    const promises = files.map((f: any, i) => {
      let uniqueId = uuidv4();
      const reader = new FileReader();
      return new Promise<void>((resolve, reject) => {
        reader.onload = (events: any) => {
          urls.push({ image: events.target.result, uniqueId: uniqueId });
          fileContents.push({ file: f, uniqueId: uniqueId });
          resolve();
        };
        reader.readAsDataURL(f);
      });
    });

    return Promise.all(promises).then(() => ({
      urls,
      fileContents,
    }));
  }

  NumbersOnly(key: any): boolean {
    let patt = /^([0-9\+.])$/;
    let result = patt.test(key);
    return result;
  }

  TrimFormInputValue(ItemForm: FormGroup) {
    Object.keys(ItemForm.value).forEach(key => {
      if (typeof (ItemForm.value[key]) == 'string') {
        ItemForm.get(key).setValue(ItemForm.value[key]?.trim())
        ItemForm.get(key).setValue(ItemForm.value[key].replace(/\s+/g, ' '))
      }
    });

    return ItemForm;
  }

  noSpaceValidator(control: FormControl) {
    if (control.value) {
      let value = control.value;
      if (typeof value === 'number') {
        value = value.toString();
      }

      if (value.trim().length === 0) {
        return { noSpace: true };
      }
    }

    if (!control.value) {
      return null;
    }


    return null;
  }

  public markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
    });
  }

  public validationMessages() {
    const messages = {
      required: 'هذا الحقل مطلوب',
      noSpace: 'لا يمكن أن يحتوي الحقل على مسافات فقط',
      email: 'Invalid email address',
      pattern: 'Invalid input pattern',
      min: 'The entered value is less than the minimum allowed',
      max: 'The entered value is greater than the maximum allowed',
      invalid_URL: 'Invalid URL',
      endDateLessThanStartDate: (error: string) => error || 'The end date must be greater than the start date',
      dateLessThan: (error: string) => error || 'The end date must be greater than the start date',
      dateGreaterThanToday: (error: string) => error || 'The end date must be greater than the start date',
      regexPattern: (error: string) => error || 'Invalid input pattern',
      arrayLength: (error: string) => error || 'Invalid number of items',
      invalidExtension: (matches: any[]) => {
        let matchedCharacters = matches;
        matchedCharacters = matchedCharacters.reduce((characterString, character, index) => {
          let string = characterString;
          string += character;

          if (matchedCharacters.length !== index + 1) {
            string += ', ';
          }

          return string;
        }, '');

        return `File extension not allowed. Allowed extensions are: ${matchedCharacters}`;
      },
      invalid_characters: (matches: any[]) => {

        let matchedCharacters = matches;

        matchedCharacters = matchedCharacters.reduce((characterString, character, index) => {
          let string = characterString;
          string += character;

          if (matchedCharacters.length !== index + 1) {
            string += ', ';
          }

          return string;
        }, '');

        return `Invalid characters: ${matchedCharacters}`;
      },
    };

    return messages;
  }

  public validateForm(formToValidate: FormGroup, formErrors: any, checkDirty?: boolean) {
    const form = formToValidate;

    for (const field in formErrors) {
      if (field) {
        formErrors[field] = '';
        const control = form.get(field);

        const messages = this.validationMessages();
        if (control && !control.valid) {
          if (!checkDirty || (control.dirty || control.touched)) {
            for (const key in control.errors) {

              if (key && !['invalid_characters', 'invalidExtension', 'endDateLessThanStartDate', 'regexPattern', 'dateGreaterThanToday', 'dateLessThan', 'arrayLength'].includes(key)) {
                formErrors[field] = formErrors[field] || messages[key];
              }
              else {
                formErrors[field] = formErrors[field] || messages[key](control.errors[key]);
              }
            }
          }
        }
      }
    }

    return formErrors;
  }
}
