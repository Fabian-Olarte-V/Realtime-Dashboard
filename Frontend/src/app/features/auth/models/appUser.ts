export type UserRole = 'ADMIN' | 'AGENT';

export interface AppUser {
  id: string;
  name: string;
  role: UserRole;
}

export interface AuthRequestPayload {
  email: string;
  password: string;
}
