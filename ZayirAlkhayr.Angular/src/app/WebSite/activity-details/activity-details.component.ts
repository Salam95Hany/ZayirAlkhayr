import { Component } from '@angular/core';
import SwiperCore, { Autoplay, Navigation, Pagination, Scrollbar, A11y } from 'swiper';
SwiperCore.use([Autoplay, Navigation, Pagination, Scrollbar, A11y]);

@Component({
  selector: 'app-activity-details',
  templateUrl: './activity-details.component.html',
  styleUrls: ['./activity-details.component.css']
})
export class ActivityDetailsComponent {

}
