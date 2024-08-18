import { Component } from '@angular/core';

@Component({
  selector: 'app-family-status',
  templateUrl: './family-status.component.html',
  styleUrls: ['./family-status.component.css']
})
export class FamilyStatusComponent {
 isDate = false;
  onfocus() {
    this.isDate = true;
  }
}
