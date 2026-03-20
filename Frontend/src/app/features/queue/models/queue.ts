export type ItemStatus = 'NEW' | 'IN_PROGRESS' | 'DONE' | 'FAILED';
export type SortOption = 'updatedat' | 'deadlineat' | 'createdat';
export type MutationStatus = 'IDLE' | 'LOADING' | 'ERROR';

export interface QueueItem {
  id: string;
  title: string;
  description?: string;
  status: ItemStatus;
  priority: 1 | 2 | 3 | 4 | 5;
  assigneeId?: string;

  deadlineAt: string;
  createdAt: string;
  updatedAt: string;

  version: number;
  failReason?: 'DEADLINE_EXCEEDED';
}

export interface QueueFilters {
  searchText: string;
  status: ItemStatus | 'ALL';
  sort: SortOption;
}

export interface TicketMutationResponse {
  items: QueueItem;
  serverTime: string;
}

export interface MutationTicketRequest {
  expectedVersion: number;
}

