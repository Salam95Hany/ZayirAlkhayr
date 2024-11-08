import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';

@Component({
  selector: 'app-project-denied',
  templateUrl: './project-denied.component.html',
  styleUrls: ['./project-denied.component.css']
})
export class ProjectDeniedComponent implements OnInit {
  ProjectsList: any[] = [];
  ProjectId: any;
  ProjectName = '';
  constructor(private route: ActivatedRoute, private adminService: AdminWebsiteService) {

  }

  ngOnInit(): void {
    this.ProjectId = this.route.snapshot.paramMap.get('id');
    this.GetAllDeniedProjects()
  }

  GetAllDeniedProjects() {
    this.adminService.GetAllDeniedProjects().subscribe(data => {
      this.ProjectsList = data;
      this.ProjectName = this.ProjectsList.find(i => i.id == this.ProjectId)?.name;
      this.ProjectsList = this.ProjectsList.filter(i => i.id != this.ProjectId && i.isVisible);
    });
  }
}
