import { Component, inject } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import * as AuthActions from './features/auth/store/auth.actions';
import { filter, startWith } from 'rxjs';
import { ErrorHandlerToast } from './shared/components/error-handler-toast/error-handler-toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ErrorHandlerToast],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  private readonly store = inject(Store);
  private readonly router = inject(Router);
  showHeader = true;

  constructor() {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        startWith(null),
      )
      .subscribe(() => {
        this.showHeader = !this.isCurrentRouteHidden();
      });
  }

  logout(): void {
    this.store.dispatch(AuthActions.logout());
  }

  private isCurrentRouteHidden(): boolean {
    let current = this.router.routerState.snapshot.root;

    while (current.firstChild) {
      current = current.firstChild;
    }

    return Boolean(current.data['hideHeader']);
  }
}
