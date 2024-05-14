import { Component } from '@angular/core';
import { Slide } from '../media/media.interface';

@Component({
  selector: 'app-photo-details',
  templateUrl: './photo-details.component.html',
  styleUrls: ['./photo-details.component.css']
})
export class PhotoDetailsComponent {
  isGallery = false;
  currentGallery: number = 0;
  imageUrls: Slide[] = [
    { src: '../../../assets/details-1.jpg' },
    { src: '../../../assets/details-2.jpg' },
    { src: '../../../assets/details-3.jpg' },
    { src: '../../../assets/details-4.jpg' },
    { src: '../../../assets/details-5.jpg' },
    { src: '../../../assets/details-6.jpg' },
    { src: '../../../assets/details-7.jpg' },
    { src: '../../../assets/details-8.jpg' }
  ];
}
