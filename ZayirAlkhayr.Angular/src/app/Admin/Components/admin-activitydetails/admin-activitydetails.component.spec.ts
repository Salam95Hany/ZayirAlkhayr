import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminActivitydetailsComponent } from './admin-activitydetails.component';

describe('AdminActivitydetailsComponent', () => {
  let component: AdminActivitydetailsComponent;
  let fixture: ComponentFixture<AdminActivitydetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminActivitydetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminActivitydetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
