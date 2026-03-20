import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { UiErrorService } from '../../shared/services/ui-error.service';

export const errorHandlerInterceptor: HttpInterceptorFn = (req, next) => {
  const uiErrorService = inject(UiErrorService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const message = error.error?.message || error.message || 'Unexpected error';
      uiErrorService.show(message);

      return throwError(() => error);
    }),
  );
};
