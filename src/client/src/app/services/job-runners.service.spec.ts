import { TestBed } from '@angular/core/testing';

import { JobRunnersService } from './job-runners.service';

describe('JobRunnersService', () => {
  let service: JobRunnersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JobRunnersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
