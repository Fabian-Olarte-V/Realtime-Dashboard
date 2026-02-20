import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthState } from './auth.models';
import { authFeatureKey } from './auth.reducer';

export const selectAuthState = createFeatureSelector<AuthState>(authFeatureKey);

export const selectUser = createSelector(selectAuthState, (s) => s.user);
export const selectUserId = createSelector(selectUser, (user) => user?.id ?? null);
export const selectToken = createSelector(selectAuthState, (s) => s.token);
export const selectUserRole = createSelector(selectUser, (user) => user?.role ?? null);
export const selectIsAuthenticated = createSelector(selectAuthState, (s) => s.authStatus);
