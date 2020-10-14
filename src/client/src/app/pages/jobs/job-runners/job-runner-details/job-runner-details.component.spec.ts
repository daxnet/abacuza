import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobRunnerDetailsComponent } from './job-runner-details.component';

describe('JobRunnerDetailsComponent', () => {
  let component: JobRunnerDetailsComponent;
  let fixture: ComponentFixture<JobRunnerDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobRunnerDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobRunnerDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
