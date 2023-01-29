import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BidTemplateDialogFormComponent } from './bid-template-dialog-form.component';

describe('BidTemplateDialogFormComponent', () => {
  let component: BidTemplateDialogFormComponent;
  let fixture: ComponentFixture<BidTemplateDialogFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BidTemplateDialogFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BidTemplateDialogFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
