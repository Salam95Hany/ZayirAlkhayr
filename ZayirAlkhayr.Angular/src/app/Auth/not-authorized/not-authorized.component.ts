import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-authorized',
  templateUrl: './not-authorized.component.html',
  styleUrls: ['./not-authorized.component.css']
})
export class NotAuthorizedComponent implements OnInit {
  UserModel: any;
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  }

  Logout() {
    this.authService.AdminLogout(this.UserModel?.userId).subscribe(data => {
      if (data) {
        localStorage.removeItem('UserModel');
        this.router.navigateByUrl('/login');
      }
    });
  }

  GoToDashboard() {
    if (this.UserModel) {
      this.router.navigateByUrl('/admin/dashboard');
      return;
    }

    this.router.navigateByUrl('/login');
  }

}
