import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-benefactor-events',
  templateUrl: './benefactor-events.component.html',
  styleUrls: ['./benefactor-events.component.css'],
  animations: [
    trigger('slideUp', [
      state('void', style({ transform: 'translateY(100%)', opacity: 0 })),
      transition(':enter', [
        animate('0.5s ease-out', style({ transform: 'translateY(0)', opacity: 1 }))
      ])
    ])
  ]
})
export class BenefactorEventsComponent implements OnInit {
  leftCounter: string = '0';
  beneFactorCounter: string = '0';
  numberToDisplay: string = '300000';
  displayedDigits: string[] = [];
  currentIndex: number = 0;

  ngOnInit() {
    this.animateCounters();
  }

  animateCounters() {
    let leftValue = 0;
    let beneFactorValue = 0;
    const leftTarget = 100;
    const beneFactorTarget = 50;

    const leftInterval = setInterval(() => {
      if (leftValue < leftTarget) {
        leftValue++;
        this.leftCounter = leftValue.toString();
      } else {
        clearInterval(leftInterval);
      }
    }, 50);

    const interval = setInterval(() => {
      if (this.currentIndex < this.numberToDisplay.length) {
        this.displayedDigits.push(this.numberToDisplay[this.currentIndex]);
        this.currentIndex++;
      } else {
        clearInterval(interval);
      }
    }, 800);

    const beneFactorInterval = setInterval(() => {
      if (beneFactorValue < beneFactorTarget) {
        beneFactorValue++;
        this.beneFactorCounter = beneFactorValue.toString();
      } else {
        clearInterval(beneFactorInterval);
      }
    }, 50);
  }


}
