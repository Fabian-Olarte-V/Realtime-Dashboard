import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import * as QueueActions from './queue.actions';
import * as QueueSelectors from './queue.selectors';
import * as AuthSelectors from '../../auth/store/auth.selectors';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { QueueService } from '../services/queue.service';
import {
  catchError,
  exhaustMap,
  filter,
  map,
  of,
  switchMap,
  takeUntil,
  timer,
  withLatestFrom,
} from 'rxjs';

@Injectable()
export class QueueEffects {
  private readonly actions$ = inject(Actions);
  private readonly store = inject(Store);
  private readonly queueApi = inject(QueueService);

  private getErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.message || fallback;
    }

    if (error instanceof Error) {
      return error.message;
    }

    return fallback;
  }

  startPolling$ = createEffect(() =>
    this.actions$.pipe(
      ofType(QueueActions.startPolling),
      switchMap(() =>
        timer(0, 30000).pipe(
          takeUntil(this.actions$.pipe(ofType(QueueActions.stopPolling))),
          withLatestFrom(this.store.select(QueueSelectors.selectLastSyncAt)),
          exhaustMap(([, lastSyncAt]) => {
            const since = lastSyncAt ?? '1970-01-01T00:00:00.000Z';
            return this.queueApi.getTickets(since).pipe(
              map((res) => QueueActions.pollSuccess({ items: res.data, lastSyncAt: res.serverTime })),
              catchError((error) =>
                of(
                  QueueActions.pollFailure({
                    error: this.getErrorMessage(error, 'Polling failed'),
                  }),
                ),
              ),
            );
          }),
        ),
      ),
    ),
  );

  searchAllFilteredTickets = createEffect(() =>
    this.actions$.pipe(
      ofType(QueueActions.setFilters),
      withLatestFrom(this.store.select(QueueSelectors.selectFilters)),
      exhaustMap(([, filter]) =>
        this.queueApi.getAllTickets(filter.searchText, filter.status, filter.sort).pipe(
          map((response) => QueueActions.searchFilteredTicketsSuccess({ items: response.data})),
          catchError((error) =>
            of(QueueActions.pollFailure({ error: error.message }))
          )
        )
      )
    )
  );

  assignToMe$ = createEffect(() =>
    this.actions$.pipe(
      ofType(QueueActions.assignToMe),
      withLatestFrom(
        this.store.select(QueueSelectors.selectSelectedTicket),
        this.store.select(AuthSelectors.selectUser),
      ),
      filter(([, ticket, user]) => !!ticket && !!user),
      exhaustMap(([, ticket]) =>
        this.queueApi
          .assignTicket(ticket!.id, { expectedVersion: ticket!.version })
          .pipe(
            map((res) =>
              QueueActions.mutationSuccess({ item: res.item, serverTime: res.serverTime }),
            ),
            catchError((error) =>
              of(
                QueueActions.mutationFailure({
                  error: this.getErrorMessage(error, 'Assignment failed'),
                }),
              ),
            ),
          ),
      ),
    ),
  );

  completeSelected$ = createEffect(() =>
    this.actions$.pipe(
      ofType(QueueActions.completeSelected),
      withLatestFrom(this.store.select(QueueSelectors.selectSelectedTicket)),
      filter(([, ticket]) => !!ticket),
      exhaustMap(([, ticket]) =>
        this.queueApi.completeTicket(ticket!.id, { expectedVersion: ticket!.version }).pipe(
          map((res) =>
            QueueActions.mutationSuccess({ item: res.item, serverTime: res.serverTime }),
          ),
          catchError((error) =>
            of(
              QueueActions.mutationFailure({
                error: this.getErrorMessage(error, 'Completion failed'),
              }),
            ),
          ),
        ),
      ),
    ),
  );
}
