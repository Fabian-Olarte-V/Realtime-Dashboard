import { AuthState } from '../features/auth/store/auth.models';
import { QueueState } from '../features/queue/store/queue.models';

export interface AppState {
  auth: AuthState;
  queue: QueueState;
}
