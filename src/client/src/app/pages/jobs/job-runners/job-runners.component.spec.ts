import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobRunnersComponent } from './job-runners.component';

describe('JobRunnersComponent', () => {
  let component: JobRunnersComponent;
  let fixture: ComponentFixture<JobRunnersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobRunnersComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobRunnersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
