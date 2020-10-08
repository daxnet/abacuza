import { TestBed } from '@angular/core/testing';

import { CommonDialogService } from './common-dialog.service';

describe('CommonDialogService', () => {
  let service: CommonDialogService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommonDialogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
