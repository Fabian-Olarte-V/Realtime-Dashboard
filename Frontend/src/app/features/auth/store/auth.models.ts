import { AppUser } from '../models/appUser';

export interface AuthState {
  user: AppUser | null;
  token: string | null;
  authStatus: boolean;
  loading: boolean;
  error: string | null;
}

export const initialAuthState: AuthState = {
  user: null,
  token: null,
  authStatus: false,
  loading: false,
  error: null,
};
