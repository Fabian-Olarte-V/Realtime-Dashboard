import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { Observable, of } from 'rxjs';
import {
  AssignTicketRequest,
  CompleteTicketRequest,
  QueueItem,
  TicketMutationResponse,
} from '../models/queue';

@Injectable({ providedIn: 'root' })
export class QueueService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/tickets`;

  getTickets(sinceIso: string): Observable<QueueItem[]> {
    // const params = new HttpParams().set('since', sinceIso);
    // return this.http.get<QueueItem[]>(this.baseUrl, { params });
    console.log('fetching tickets updated since', sinceIso);
    return of(makeMockTickets(5));
  }

  assignTicket(id: string, body: AssignTicketRequest): Observable<TicketMutationResponse> {
    // return this.http.post<TicketMutationResponse>(`${this.baseUrl}/${id}/assign`, body);
    console.log(`assigning ticket ${id} to ${body.assigneeId}`);
    return of({
      item: makeMockTickets(1)[0],
      serverTime: new Date().toISOString(),
    });
  }

  completeTicket(id: string, body: CompleteTicketRequest): Observable<TicketMutationResponse> {
    // return this.http.post<TicketMutationResponse>(`${this.baseUrl}/${id}/complete`, body);
    console.log(`completing ticket ${id} with version ${body.expectedVersion}`);
    return of({
      item: makeMockTickets(1)[0],
      serverTime: new Date().toISOString(),
    });
  }
}

function makeMockTickets(count: number): QueueItem[] {
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
