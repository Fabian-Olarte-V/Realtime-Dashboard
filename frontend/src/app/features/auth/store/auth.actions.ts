import { createAction, props } from '@ngrx/store';
import { AppUser, AuthRequestPayload } from '../models/appUser';

export const login = createAction('[Auth] Login', props<{ user: AuthRequestPayload }>());
export const signup = createAction('[Auth] Signup', props<{ user: AuthRequestPayload }>());

export const updateUserSession = createAction(
  '[Auth] Update User Session',
  props<{ user: AppUser; token: string }>(),
);

export const logout = createAction('[Auth] Logout');
