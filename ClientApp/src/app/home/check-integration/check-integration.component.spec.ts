import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckIntegrationComponent } from './check-integration.component';

describe('CheckIntegrationComponent', () => {
  let component: CheckIntegrationComponent;
  let fixture: ComponentFixture<CheckIntegrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CheckIntegrationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CheckIntegrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
