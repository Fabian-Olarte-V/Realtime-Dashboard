import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { Store } from '@ngrx/store';
import * as AuthActions from '../../features/auth/store/auth.actions';

export const authErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const store = inject(Store);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401 && !req.url.includes('auth')) {
        store.dispatch(AuthActions.logout());
        router.navigate(['/login']);
      }
      return throwError(() => err);
    }),
  );
};
