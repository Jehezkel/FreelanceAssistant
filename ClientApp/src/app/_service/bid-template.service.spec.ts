import { TestBed } from '@angular/core/testing';

import { BidTemplateService } from './bid-template.service';

describe('BidTemplateService', () => {
  let service: BidTemplateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BidTemplateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
