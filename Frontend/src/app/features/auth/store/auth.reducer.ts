import { createReducer, on } from '@ngrx/store';
import { initialAuthState } from './auth.models';
import * as AuthActions from './auth.actions';

export const authFeatureKey = 'auth';

export const authReducer = createReducer(
  initialAuthState,

  on(AuthActions.login, AuthActions.signup, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(
    AuthActions.loginSuccess,
    AuthActions.signupSuccess,
    AuthActions.updateUserSession,
    (state, { user, token }) => ({
      ...state,
      user,
      token,
      authStatus: true,
      loading: false,
      error: null,
    }),
  ),
  
  on(AuthActions.loginFailure, AuthActions.signupFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  on(AuthActions.clearAuthError, (state) => ({
    ...state,
    error: null,
  })),

  on(AuthActions.logout, () => initialAuthState),
);
