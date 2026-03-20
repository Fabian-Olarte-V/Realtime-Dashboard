import { ActionReducer, INIT, MetaReducer, UPDATE } from '@ngrx/store';
import { AuthState, initialAuthState } from '../features/auth/store/auth.models';
import { authFeatureKey } from '../features/auth/store/auth.reducer';

const AUTH_STORAGE_KEY = 'auth_state';

function loadPersistedAuthState(): Partial<AuthState> | null {
  try {
    const serializedState = localStorage.getItem(AUTH_STORAGE_KEY);
    if (!serializedState) return null;

    const parsedState = JSON.parse(serializedState) as Partial<AuthState>;

    return {
      user: parsedState.user ?? null,
      token: parsedState.token ?? null,
      authStatus: parsedState.authStatus ?? false,
      loading: false,
      error: null,
    };
  } catch {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    return null;
  }
}

function persistAuthState(authState: AuthState | undefined): void {
  if (!authState || !authState.authStatus || !authState.token || !authState.user) {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    return;
  }

  const stateToPersist = {
    user: authState.user,
    token: authState.token,
    authStatus: authState.authStatus,
  };

  localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(stateToPersist));
}

export function authStorageMetaReducer<State>(
  reducer: ActionReducer<State>,
): ActionReducer<State> {
  return (state, action) => {
    const isHydrationAction = action.type === INIT || action.type === UPDATE;

    const stateWithHydratedAuth = isHydrationAction
      ? ({
          ...state,
          [authFeatureKey]: {
            ...initialAuthState,
            ...loadPersistedAuthState(),
          },
        } as State)
      : state;

    const nextState = reducer(stateWithHydratedAuth, action);
    const authState = (nextState as Record<string, AuthState | undefined>)[authFeatureKey];

    persistAuthState(authState);

    return nextState;
  };
}

export const metaReducers: MetaReducer[] = [authStorageMetaReducer];
