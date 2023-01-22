import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSearchesComponent } from './project-searches.component';

describe('ProjectSearchesComponent', () => {
  let component: ProjectSearchesComponent;
  let fixture: ComponentFixture<ProjectSearchesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectSearchesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectSearchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
