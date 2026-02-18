import { createReducer, on } from '@ngrx/store';
import { initialAuthState } from './auth.models';
import * as AuthActions from './auth.actions';

export const authFeatureKey = 'auth';

export const authReducer = createReducer(
  initialAuthState,
  
  on(AuthActions.updateUserSession, (state, { user, token }) => ({
    ...state,
    user,
    token,
    authStatus: true,
  })),

  on(AuthActions.logout, () => initialAuthState),
);
