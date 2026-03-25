import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginFormComponent } from './login-form';

describe('LoginFormComponent', () => {
  let component: LoginFormComponent;
  let fixture: ComponentFixture<LoginFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not emit login when the form is invalid', () => {
    const emitSpy = vi.spyOn(component.submitLogin, 'emit');

    component.onSubmit();

    expect(component.submitted).toBe(true);
    expect(emitSpy).not.toHaveBeenCalled();
    expect(component.loginForm.touched).toBe(true);
  });

  it('should emit login payload when the form is valid', () => {
    const emitSpy = vi.spyOn(component.submitLogin, 'emit');

    component.loginForm.setValue({
      username: 'agent01',
      password: 'password123',
    });

    component.onSubmit();

    expect(emitSpy).toHaveBeenCalledWith({
      username: 'agent01',
      password: 'password123',
    });
  });
});
