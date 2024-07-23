import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ValidationFormService } from 'src/app/Admin/Services/validation-form.service';
import { BenefactorauthService } from 'src/app/Auth/benefactorauth.service';

@Component({
  selector: 'app-benefactor-login',
  templateUrl: './benefactor-login.component.html',
  styleUrls: ['./benefactor-login.component.css']
})
export class BenefactorLoginComponent {
  @ViewChild('LoginForm') LoginForm: any;
  BeneFactorLogin: BeneFactorLogin = {} as BeneFactorLogin;
  ErrorMessage = '';
  ButtonDisabled = false;
  isShowPassword = false;
  constructor(private benefactorAuthService: BenefactorauthService, private router: Router, private formService: ValidationFormService) {

  }

  Login() {
    this.LoginForm.onSubmit();
    const isValid = this.LoginForm.form.valid;
    if (!isValid)
      return;

    this.ButtonDisabled = true;
    this.benefactorAuthService.BeneFactorLogin(Number(this.BeneFactorLogin.code), this.BeneFactorLogin.name).subscribe(data => {
      this.ButtonDisabled = false;
      if (data.responseCode == 200) {
        localStorage.setItem('BeneFactorModel', JSON.stringify(data));
        this.router.navigateByUrl('/benefactor-details');
      } else
        this.ErrorMessage = data.responseMessage;
    });
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }
}

export interface BeneFactorLogin {
  code: string,
  name: string,
}

