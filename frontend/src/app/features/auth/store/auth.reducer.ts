import { createReducer, on } from '@ngrx/store';
import { initialAuthState } from './auth.models';
import * as AuthActions from './auth.actions';

export const authFeatureKey = 'auth';

export const authReducer = createReducer(
  initialAuthState,
  on(AuthActions.loginMock, (state, { user, token }) => ({
    ...state,
    user,
    token,
    status: 'authenticated',
  })),
  
  on(AuthActions.logout, () => initialAuthState),
);
