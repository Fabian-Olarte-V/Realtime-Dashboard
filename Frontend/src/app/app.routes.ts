import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    data: { hideHeader: true },
    loadComponent: () =>
      import('./features/auth/pages/auth-page/auth-page').then((m) => m.AuthPage),
  },
  {
    path: 'queue',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/queue/pages/queue-page/queue-page').then((m) => m.QueuePage),
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
