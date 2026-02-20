import { createReducer, on } from '@ngrx/store';
import { initialQueueState } from './queue.models';
import * as QueueActions from './queue.actions';

export const queueFeatureKey = 'queue';

export const queueReducer = createReducer(
  initialQueueState,
  on(QueueActions.setFilters, (state, { filters }) => ({
    ...state,
    filters: filters,
  })),

  on(QueueActions.selectTicket, (state, { id }) => ({
    ...state,
    selectedItemId: id,
  })),

  on(QueueActions.startPolling, (state) => ({
    ...state,
    pollingEnabled: true,
    pollingError: null,
  })),

  on(QueueActions.stopPolling, (state) => ({
    ...state,
    pollingEnabled: false,
  })),

  on(QueueActions.pollSuccess, (state, { items }) => {
    const entities = { ...state.items };
    const ids = new Set(state.ids);

    for (const item of items) {
      entities[item.id] = item;
      if (!ids.has(item.id)) ids.add(item.id);
    }

    return {
      ...state,
      items: entities,
      ids: Array.from(ids),
      lastSyncAt: "",
      pollingError: null,
    };
  }),

  on(QueueActions.pollFailure, (state, { error }) => ({
    ...state,
    pollingError: error,
  })),

  on(QueueActions.mutationSuccess, (state, { item, serverTime }) => {
    const entities = { ...state.items, [item.id]: item };
    const ids = state.ids.includes(item.id) ? state.ids : [...state.ids, item.id];

    return {
      ...state,
      items: entities,
      ids,
      lastSyncAt: serverTime,
      mutationStatus: 'IDLE',
      mutationError: null,
    };
  }),

  on(QueueActions.mutationFailure, (state, { error }) => ({
    ...state,
    mutationStatus: 'ERROR',
    mutationError: error,
  })),

  on(QueueActions.clearMutationFailure, (state) => ({
    ...state,
    mutationStatus: 'IDLE',
    mutationError: null,
  })),
);
