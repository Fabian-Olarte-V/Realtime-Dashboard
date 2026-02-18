import { Component, inject } from '@angular/core';
import { LoginFormComponent, LoginPayload } from '../../components/login-form/login-form';
import {
  RegisterFormComponent,
  RegisterPayload,
} from '../../components/register-form/register-form';
import { Store } from '@ngrx/store';
import * as AuthActions from '../../store/auth.actions';

@Component({
  selector: 'app-auth-page',
  standalone: true,
  imports: [LoginFormComponent, RegisterFormComponent],
  templateUrl: './auth-page.html',
  styleUrl: './auth-page.scss',
})
export class AuthPage {
  private readonly store = inject(Store);
  isLoginMode = true;

  showRegister(): void {
    this.isLoginMode = false;
  }

  showLogin(): void {
    this.isLoginMode = true;
  }

  onLoginSubmit(payload: LoginPayload): void {
    this.store.dispatch(AuthActions.login({ user: payload }));
  }

  onRegisterSubmit(payload: RegisterPayload): void {
    this.store.dispatch(AuthActions.signup({ user: payload }));
  }
}
