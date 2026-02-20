import { authFeatureKey, authReducer } from '../features/auth/store/auth.reducer';
import { queueFeatureKey, queueReducer } from '../features/queue/store/queue.reducer';

export const appReducers = {
  [authFeatureKey]: authReducer,
  [queueFeatureKey]: queueReducer
};
