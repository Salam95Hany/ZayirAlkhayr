import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeSlideimageComponent } from './home-slideimage.component';

describe('HomeSlideimageComponent', () => {
  let component: HomeSlideimageComponent;
  let fixture: ComponentFixture<HomeSlideimageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeSlideimageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeSlideimageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
