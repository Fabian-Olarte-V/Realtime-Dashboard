import { QueueItem } from '../models/queue';

export function makeMockTickets(count: number): QueueItem[] {
  const now = Date.now();
  const statuses: QueueItem['status'][] = ['NEW', 'IN_PROGRESS', 'DONE', 'FAILED'];

  return Array.from({ length: count }).map((_, i) => {
    const createdAt = new Date(now - i * 60_000).toISOString();
    const updatedAt = new Date(now - i * 30_000).toISOString();
    const deadlineAt = new Date(now + (i % 50) * 60_000).toISOString();

    return {
      id: `T-${i + 1}`,
      title: `Ticket ${i + 1}`,
      description: `This is ticket ${i + 1}`,
      status: statuses[i % statuses.length],
      priority: ((i % 5) + 1) as 1 | 2 | 3 | 4 | 5,
      assigneeId: i % 3 === 0 ? 'agent-1' : undefined,
      deadlineAt,
      createdAt,
      updatedAt,
      version: 1,
      failReason: i % statuses.length === 3 ? 'DEADLINE_EXCEEDED' : undefined,
    };
  });
}

export function compareIsoDesc(a: string, b: string): number {
  return a > b ? -1 : a < b ? 1 : 0;
}

export function compareIsoAsc(a: string, b: string): number {
  return a > b ? 1 : a < b ? -1 : 0;
}

export function compareNumDesc(a: number, b: number): number {
  return b - a;
}
