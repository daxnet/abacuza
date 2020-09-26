import { TestBed } from '@angular/core/testing';

import { ClustersService } from './clusters.service';

describe('ClustersService', () => {
  let service: ClustersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClustersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
