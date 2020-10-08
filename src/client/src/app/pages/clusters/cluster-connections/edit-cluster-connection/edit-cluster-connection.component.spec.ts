import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditClusterConnectionComponent } from './edit-cluster-connection.component';

describe('EditClusterConnectionComponent', () => {
  let component: EditClusterConnectionComponent;
  let fixture: ComponentFixture<EditClusterConnectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditClusterConnectionComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditClusterConnectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
