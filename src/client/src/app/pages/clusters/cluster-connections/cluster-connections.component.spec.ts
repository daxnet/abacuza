import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClusterConnectionsComponent } from './cluster-connections.component';

describe('ClusterConnectionsComponent', () => {
  let component: ClusterConnectionsComponent;
  let fixture: ComponentFixture<ClusterConnectionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClusterConnectionsComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClusterConnectionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
