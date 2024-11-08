import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminWebsiteService } from 'src/app/Admin/Services/admin-website.service';
import { trigger, state, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
  animations: [
    trigger('slideUp', [
      state('void', style({ transform: 'translateY(100%)', opacity: 0 })),
      transition(':enter', [
        animate('0.5s ease-out', style({ transform: 'translateY(0)', opacity: 1 }))
      ])
    ])
  ]
})
export class ProjectsComponent implements OnInit {
  collapsed = true;
  beneFactorCounter: string;
  DonationAmountToDisplay: string;
  DonationAmountToDisplay2: string;
  DonationAmountToDisplay3: string;
  DonationAmountDigits: string[] = [];
  DonationAmountDigits2: string[] = [];
  DonationAmountDigits3: string[] = [];
  currentIndex: number = 0;
  currentIndex2: number = 0;
  currentIndex3: number = 0;
  ProjectId: any;
  ProjectData: any;
  constructor(private adminService: AdminWebsiteService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.ProjectId = this.route.snapshot.paramMap.get('id');
    this.GetWebSiteProjectsById();
  }

  GetWebSiteProjectsById() {
    this.adminService.GetWebSiteProjectsById(this.ProjectId).subscribe(data => {
      this.ProjectData = data;
      this.DonationAmountToDisplay = this.ProjectData.totalDonationAmount;
      this.DonationAmountToDisplay2 = this.ProjectData.totalAmount;
      this.DonationAmountToDisplay3 = this.ProjectData.remainingAmount;
      this.animateCounters();
    })
  }

  animateCounters() {
    let BenefactorCount = 0;
    const BenefactorCountTarget = Number(this.ProjectData.benefactorCount);

    const interval = setInterval(() => {
      if (this.currentIndex < this.DonationAmountToDisplay.length) {
        this.DonationAmountDigits.unshift(this.DonationAmountToDisplay[this.currentIndex]);
        this.currentIndex++;
      } else {
        clearInterval(interval);
      }
    }, 700);

    const interval2 = setInterval(() => {
      if (this.currentIndex2 < this.DonationAmountToDisplay2.length) {
        this.DonationAmountDigits2.unshift(this.DonationAmountToDisplay2[this.currentIndex2]);
        this.currentIndex2++;
      } else {
        clearInterval(interval);
      }
    }, 700);

    const interval3 = setInterval(() => {
      if (this.currentIndex3 < this.DonationAmountToDisplay3.length) {
        this.DonationAmountDigits3.unshift(this.DonationAmountToDisplay3[this.currentIndex3]);
        this.currentIndex3++;
      } else {
        clearInterval(interval);
      }
    }, 700);

    const beneFactorInterval = setInterval(() => {
      if (BenefactorCount < BenefactorCountTarget) {
        BenefactorCount++;
        this.beneFactorCounter = BenefactorCount.toString();
      } else {
        clearInterval(beneFactorInterval);
      }
    }, 75);
  }


}
