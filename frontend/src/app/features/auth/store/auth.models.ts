import { AppUser } from '../models/appUser';

export interface AuthState {
  user: AppUser | null;
  token: string | null;
  authStatus: boolean;
}

export const initialAuthState: AuthState = {
  user: null,
  token: null,
  authStatus: false
};
