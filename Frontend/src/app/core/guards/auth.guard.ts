import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, take } from 'rxjs';
import * as AuthSelectors from '../../features/auth/store/auth.selectors';
import { UiErrorService } from '../../shared/services/ui-error.service';

export const authGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);
  const uiErrorToastService = inject(UiErrorService);

  return store.select(AuthSelectors.selectIsAuthenticated).pipe(
    take(1),
    map((isAuthenticated) => {
      if (isAuthenticated) {
        return true;
      } else {
        uiErrorToastService.show('You must be logged in to access this page.');
        return router.createUrlTree(['/login']);
      }
    }),
  );
};
