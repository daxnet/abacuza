import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateClusterConnectionComponent } from './create-cluster-connection.component';

describe('CreateClusterConnectionComponent', () => {
  let component: CreateClusterConnectionComponent;
  let fixture: ComponentFixture<CreateClusterConnectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateClusterConnectionComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateClusterConnectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
