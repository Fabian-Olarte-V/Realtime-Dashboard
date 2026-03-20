import { HttpClient, HttpParams } from '@angular/common/http';
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
import { ApiResponse } from '../../../shared/models/apiResponse/apiResponse';

@Injectable({providedIn: 'root'})
export class QueueService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/tickets`;

  getAllTickets(q?: string, status?: string, sortby?: string): Observable<ApiResponse<QueueItem[]>>{
    const params = new HttpParams()
                        .set('status', status ?? "")
                        .set('query', q ?? "")
                        .set('sort', sortby ?? "");

    return this.http.get<ApiResponse<QueueItem[]>>(`${this.baseUrl}`, { params });
  }

  getTickets(sinceIso: string): Observable<ApiResponse<QueueItem[]>> {
    const params = new HttpParams().set('sinceiso', sinceIso);
    return this.http.get<ApiResponse<QueueItem[]>>(`${this.baseUrl}/changes`, { params });
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
