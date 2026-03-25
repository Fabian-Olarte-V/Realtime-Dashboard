import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { AuthRequestPayload, AuthUser, SignupRequestPayload } from "../models/appUser";
import { ApiResponse } from "../../../shared/models/apiResponse/apiResponse";


@Injectable({providedIn: 'root'})
export class AuthService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/auth`;
    
    login(authRequest: AuthRequestPayload): Observable<ApiResponse<AuthUser>> {
        return this.http.post<ApiResponse<AuthUser>>(`${this.baseUrl}/login`, authRequest);
    }

    signup(authRequest: SignupRequestPayload): Observable<ApiResponse<AuthUser>> {
        return this.http.post<ApiResponse<AuthUser>>(`${this.baseUrl}/signup`, authRequest);
    }
}
