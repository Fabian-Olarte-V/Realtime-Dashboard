import { CommonModule } from '@angular/common';
import { Component, inject, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EyeIconComponent } from '../../../../shared/components/icons/eye-icon/eye-icon';
import { EyeOffIconComponent } from '../../../../shared/components/icons/eye-off-icon/eye-off-icon';

export interface LoginPayload {
  email: string;
  password: string;
}

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, EyeIconComponent, EyeOffIconComponent],
  templateUrl: './login-form.html',
  styleUrl: './login-form.scss',
})
export class LoginFormComponent {
  private readonly formBuilder = inject(FormBuilder);

  readonly submitLogin = output<LoginPayload>();
  readonly signUpClicked = output<void>();

  readonly loginForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  showPassword = false;
  submitted = false;

  get emailControl() {
    return this.loginForm.controls.email;
  }

  get passwordControl() {
    return this.loginForm.controls.password;
  }

  toggleShowPassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSignUp(): void {
    this.signUpClicked.emit();
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.submitLogin.emit(this.loginForm.getRawValue());
  }
}
