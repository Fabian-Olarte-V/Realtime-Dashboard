import { MutationStatus, QueueFilters, QueueItem } from '../models/queue';

export interface QueueState {
  items: Record<string, QueueItem>;
  selectedItemId: string | null;
  filters: QueueFilters;

  // sorting order
  ids: string[];

  //polling
  pollingEnabled: boolean;
  pollingIntervalMs: number;
  lastSyncAt: string | null;

  //polling error handling
  pollingError: string | null;

  //mutation error handling
  mutationStatus: MutationStatus;
  mutationError: string | null;
}

export const initialQueueState: QueueState = {
  items: {},
  selectedItemId: null,
  filters: { searchText: '', sort: 'updatedAt_desc', status: 'ALL' },
  ids: [],
  pollingEnabled: false,
  pollingIntervalMs: 5000,
  lastSyncAt: null,
  pollingError: null,
  mutationStatus: 'IDLE',
  mutationError: null,
};
