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
import { makeMockTickets } from '../helpers/helpers';

@Injectable({providedIn: 'root'})
export class QueueService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/tickets`;

  getTickets(sinceIso: string): Observable<QueueItem[]> {
    // const params = new HttpParams().set('since', sinceIso);
    // return this.http.get<QueueItem[]>(this.baseUrl, { params });
    console.log('fetching tickets updated since', sinceIso);
    return of(makeMockTickets(20));
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
