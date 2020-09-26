import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClusterTypesComponent } from './cluster-types.component';

describe('ClusterTypesComponent', () => {
  let component: ClusterTypesComponent;
  let fixture: ComponentFixture<ClusterTypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClusterTypesComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClusterTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
