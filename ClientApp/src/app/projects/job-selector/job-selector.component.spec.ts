import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobSelectorComponent } from './job-selector.component';

describe('JobSelectorComponent', () => {
  let component: JobSelectorComponent;
  let fixture: ComponentFixture<JobSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JobSelectorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JobSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
