import { CommonModule } from '@angular/common';
import { Component, inject, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EyeIconComponent } from '../../../../shared/components/icons/eye-icon/eye-icon';
import { EyeOffIconComponent } from '../../../../shared/components/icons/eye-off-icon/eye-off-icon';

export interface RegisterPayload {
  name: string;
  email: string;
  password: string;
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

  readonly submitRegister = output<RegisterPayload>();
  readonly loginClicked = output<void>();

  readonly registerForm = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  showPassword = false;
  submitted = false;

  get nameControl() {
    return this.registerForm.controls.name;
  }

  get emailControl() {
    return this.registerForm.controls.email;
  }

  get passwordControl() {
    return this.registerForm.controls.password;
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

    this.submitRegister.emit(this.registerForm.getRawValue());
  }
}


