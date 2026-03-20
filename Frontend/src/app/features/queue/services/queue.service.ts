import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environments';
import { Observable } from 'rxjs';
import {
  MutationTicketRequest,
  QueueItem,
  TicketMutationResponse,
} from '../models/queue';
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

  assignTicket(id: string, body: MutationTicketRequest): Observable<ApiResponse<TicketMutationResponse>> {
    return this.http.put<ApiResponse<TicketMutationResponse>>(`${this.baseUrl}/${id}/assing`, body);
  }

  completeTicket(id: string, body: MutationTicketRequest): Observable<ApiResponse<TicketMutationResponse>> {
    return this.http.put<ApiResponse<TicketMutationResponse>>(`${this.baseUrl}/${id}/complete`, body);
  }
}
