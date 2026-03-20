import { CommonModule } from '@angular/common';
import { Component, inject, output } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserRole } from '../../models/appUser';
import { EyeIconComponent } from '../../../../shared/components/icons/eye-icon/eye-icon';
import { EyeOffIconComponent } from '../../../../shared/components/icons/eye-off-icon/eye-off-icon';

export interface RegisterPayload {
  username: string;
  password: string;
  role: UserRole;
}

@Component({
  selector: 'app-register-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, EyeIconComponent, EyeOffIconComponent],
  templateUrl: './register-form.html',
  styleUrl: './register-form.scss',
})
export class RegisterFormComponent {
  private readonly formBuilder = inject(FormBuilder);
  protected readonly roles: UserRole[] = ['ADMIN', 'AGENT'];

  readonly submitRegister = output<RegisterPayload>();
  readonly loginClicked = output<void>();

  readonly registerForm = this.formBuilder.group({
    username: this.formBuilder.nonNullable.control('', [Validators.required]),
    password: this.formBuilder.nonNullable.control('', [
      Validators.required,
      Validators.minLength(8),
    ]),
    role: new FormControl<UserRole | ''>('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  showPassword = false;
  submitted = false;

  get usernameControl() {
    return this.registerForm.controls.username;
  }

  get passwordControl() {
    return this.registerForm.controls.password;
  }

  get roleControl() {
    return this.registerForm.controls.role;
  }

  toggleShowPassword(): void {
    this.showPassword = !this.showPassword;
  }

  onLogin(): void {
    this.loginClicked.emit();
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.submitRegister.emit(this.registerForm.getRawValue() as RegisterPayload);
  }
}


