import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/auth-page/auth-page').then((m) => m.AuthPage),
  },
  {
    path: 'queue',
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
