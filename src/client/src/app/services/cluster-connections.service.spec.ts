import { TestBed } from '@angular/core/testing';

import { ClusterConnectionsService } from './cluster-connections.service';

describe('ClusterConnectionsService', () => {
  let service: ClusterConnectionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClusterConnectionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
