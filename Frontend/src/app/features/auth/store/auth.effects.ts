import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
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

  private getErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.message || 'Unexpected authentication error.';
    }

    if (error instanceof Error) {
      return error.message;
    }

    return 'Unexpected authentication error.';
  }

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(({ user }) =>
        this.authService.login(user).pipe(
          map((response) =>
            AuthActions.loginSuccess({
              user: response.data.user,
              token: response.data.token,
            }),
          ),
          catchError((error) =>
            of(AuthActions.loginFailure({ error: this.getErrorMessage(error) })),
          ),
        ),
      ),
    ),
  );

  signup$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.signup),
      switchMap(({ user }) =>
        this.authService.signup(user).pipe(
          map((response) =>
            AuthActions.signupSuccess({
              user: response.data.user,
              token: response.data.token,
            }),
          ),
          catchError((error) =>
            of(AuthActions.signupFailure({ error: this.getErrorMessage(error) })),
          ),
        ),
      ),
    ),
  );

  authSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess, AuthActions.signupSuccess, AuthActions.updateUserSession),
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
