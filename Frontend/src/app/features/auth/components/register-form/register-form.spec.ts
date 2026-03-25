import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterFormComponent } from './register-form';

describe('RegisterFormComponent', () => {
  let component: RegisterFormComponent;
  let fixture: ComponentFixture<RegisterFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not emit register when the form is invalid', () => {
    const emitSpy = vi.spyOn(component.submitRegister, 'emit');

    component.onSubmit();

    expect(component.submitted).toBe(true);
    expect(emitSpy).not.toHaveBeenCalled();
    expect(component.registerForm.touched).toBe(true);
  });

  it('should emit register payload when the form is valid', () => {
    const emitSpy = vi.spyOn(component.submitRegister, 'emit');

    component.registerForm.setValue({
      username: 'admin01',
      password: 'password123',
      role: 'ADMIN',
    });

    component.onSubmit();

    expect(emitSpy).toHaveBeenCalledWith({
      username: 'admin01',
      password: 'password123',
      role: 'ADMIN',
    });
  });
});
