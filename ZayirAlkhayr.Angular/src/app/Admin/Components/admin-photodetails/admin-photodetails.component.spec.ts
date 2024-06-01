import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPhotodetailsComponent } from './admin-photodetails.component';

describe('AdminPhotodetailsComponent', () => {
  let component: AdminPhotodetailsComponent;
  let fixture: ComponentFixture<AdminPhotodetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminPhotodetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPhotodetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
