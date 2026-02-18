import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AuthService } from '../services/auth.service';
import * as AuthActions from './auth.actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class AuthEffects {
  private readonly actions$ = inject(Actions);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(({ user }) =>
        this.authService.login(user).pipe(
          map((userData) => AuthActions.updateUserSession({ user: userData, token: 'mock-token' })),
          catchError(() => {
            return of(AuthActions.logout());
          }),
        ),
      ),
    ),
  );

  signup$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.signup),
      switchMap(({ user }) =>
        this.authService.signup(user).pipe(
          map((userData) => AuthActions.updateUserSession({ user: userData, token: 'mock-token' })),
          catchError(() => {
            return of(AuthActions.logout());
          }),
        ),
      ),
    ),
  );

  authSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.updateUserSession),
        tap(() => {
          this.router.navigate(['/queue']);
        }),
      ),
    { dispatch: false },
  );

  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logout),
        tap(() => {
          this.router.navigate(['/login']);
        }),
      ),
    { dispatch: false },
  );
}
