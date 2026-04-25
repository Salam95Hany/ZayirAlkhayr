import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../Auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @ViewChild('LoginForm') LoginForm: any;
  isShowPassword = false;
  LoginModel: LoginModel = {} as LoginModel;
  ErrorMessage = '';
  ButtonDisabled = false;
  returnUrl = '/';
  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.returnUrl = this.authService.resolveReturnUrl(this.route.snapshot.queryParamMap.get('returnUrl'));
  }

  Login() {
    if (this.ButtonDisabled) return;
    const isValid = this.LoginForm.form.valid;
    if (!isValid)
      return;

    this.ButtonDisabled = true;
    this.authService.AdminLogin(this.LoginModel).subscribe(data => {
      this.ButtonDisabled = false;
      if (data.isSuccess) {
        localStorage.setItem('UserModel', JSON.stringify(data.results));
        this.router.navigateByUrl(this.returnUrl);
      } else
        this.ErrorMessage = data.message;
    });
  }
}

export interface LoginModel {
  userName: string,
  password: string,
}
