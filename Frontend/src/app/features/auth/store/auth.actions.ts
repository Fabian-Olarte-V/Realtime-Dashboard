import { createAction, props } from '@ngrx/store';
import { AppUser, AuthRequestPayload, SignupRequestPayload } from '../models/appUser';

export const login = createAction('[Auth] Login', props<{ user: AuthRequestPayload }>());
export const loginFailure = createAction('[Auth] Login Failure', props<{ error: string }>());
export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ user: AppUser; token: string }>(),
);


export const signup = createAction('[Auth] Signup', props<{ user: SignupRequestPayload }>());
export const signupFailure = createAction('[Auth] Signup Failure', props<{ error: string }>());
export const signupSuccess = createAction(
  '[Auth] Signup Success',
  props<{ user: AppUser; token: string }>(),
);


export const updateUserSession = createAction(
  '[Auth] Update User Session',
  props<{ user: AppUser; token: string }>(),
);
export const logout = createAction('[Auth] Logout');


export const clearAuthError = createAction('[Auth] Clear Error');