import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorHandlerToast } from './error-handler-toast';

describe('ErrorHandlerToast', () => {
  let component: ErrorHandlerToast;
  let fixture: ComponentFixture<ErrorHandlerToast>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ErrorHandlerToast]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ErrorHandlerToast);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
