import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateJobRunnerComponent } from './create-job-runner.component';

describe('CreateJobRunnerComponent', () => {
  let component: CreateJobRunnerComponent;
  let fixture: ComponentFixture<CreateJobRunnerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateJobRunnerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateJobRunnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
