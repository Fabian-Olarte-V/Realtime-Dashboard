import { AppUser, UserRole } from "../models/appUser";

export function createRandomAppUser(): AppUser {
  const roles: UserRole[] = ['ADMIN', 'AGENT'];
  const randomRole = roles[Math.floor(Math.random() * roles.length)];
  const randomId = Math.random().toString(36).slice(2, 10);
  const randomName = `User_${Math.random().toString(36).slice(2, 7)}`;

  return {
    id: randomId,
    username: randomName,
    role: randomRole,
  };
}