import { createAction, props } from '@ngrx/store';
import { QueueFilters, QueueItem } from '../models/queue';

export const setFilters = createAction('[Queue] Set Filters', props<{ filters: QueueFilters }>());
export const selectTicket = createAction('[Queue] Select Ticket', props<{ id: string | null }>());

export const startPolling = createAction('[Queue] Start Polling');
export const stopPolling = createAction('[Queue] Stop Polling');

export const pollSuccess = createAction('[Queue] Poll Success', props<{ items: QueueItem[] }>());
export const pollFailure = createAction('[Queue] Poll Failure', props<{ error: string }>());

export const assignToMe = createAction('[Queue] Assign To Me');
export const assignToUser = createAction('[Queue] Assign To User', props<{ assigneeId: string }>());
export const completeSelected = createAction('[Queue] Complete Selected Ticket');

export const mutationSuccess = createAction(
  '[Queue] Mutation Success',
  props<{ item: QueueItem; serverTime: string }>(),
);
export const mutationFailure = createAction(
  '[Queue] Mutation Failure',
  props<{ error: string; code?: number }>(),
);

export const clearMutationFailure = createAction('[Queue] Clear Mutation Failure');
