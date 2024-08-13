import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/Admin/Services/admin.service';

@Component({
  selector: 'app-daily-tasks',
  templateUrl: './daily-tasks.component.html',
  styleUrls: ['./daily-tasks.component.css']
})
export class DailyTasksComponent implements OnInit {
  TasksData: any[] = [];
  UserModel: any;
  showLoader = false;
  TotalCount = 0;
  UserId: any;
  TaskId: any;

  constructor(private adminService: AdminService, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
    this.GetAllUserTasks();
  }

  GetAllUserTasks() {
    this.adminService.GetAllUserTasks(this.UserModel?.userId).subscribe(data => {
      this.TasksData = data;
      this.TotalCount = data && data.length > 0 ? data[0].totalCount : 0;
    });
  }

  ConvertTaskStatus(taskId: any, statusId: any) {
    this.showLoader = true;
    this.adminService.ConvertTaskStatus(taskId, statusId).subscribe(data => {
      if (data.done) {
        this.toaster.success(data.message);
        this.GetAllUserTasks();
      }
      else
        this.toaster.error(data.message);
      this.showLoader = false;
    });
  }
}
