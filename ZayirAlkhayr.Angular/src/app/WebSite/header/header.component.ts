import { Component, ViewChild } from '@angular/core';
import { Slide } from "../media/media.interface";
import { AnimationType } from "../media/media.animations";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  @ViewChild("navbar") navbarEl: HTMLElement;

  collapsed = true;
  animationType = AnimationType.Scale;

  slides: Slide[] = [
    {
      headline: "صدقتك الجارية",
      src:
        "../../../assets/khairy-1.jpg"
    },
    {
      headline: "مساعدة المحتاجين",
      src:
        "../../../assets/khairy-2.jpg"
    },
    {
      headline: "بنك الطعام",
      src:
        "../../../assets/khairy-3.jpg"
    },
    {
      headline: "اغاثه الاخرين",
      src:
        "../../../assets/khairy-4.jpg"
    },
    {
      headline: "زكاتك المستحقه",
      src:
        "../../../assets/khairy-5.jpg"
    }
  ];
}
