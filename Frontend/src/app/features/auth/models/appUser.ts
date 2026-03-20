export type UserRole = 'ADMIN' | 'AGENT';

export interface AuthUser{
  token: string;
  user: AppUser;
}

export interface AppUser {
  id: string;
  username: string;
  role: UserRole;
}

export interface AuthRequestPayload {
  username: string;
  password: string;
}

export interface SignupRequestPayload extends AuthRequestPayload {
  role: UserRole;
}
