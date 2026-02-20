import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environments";
import { Observable, of } from "rxjs";
import { AppUser, AuthRequestPayload } from "../models/appUser";
import { createRandomAppUser } from "../helpers/helpers";


@Injectable({providedIn: 'root'})
export class AuthService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiBaseUrl}/auth`;
    
    login(authRequest: AuthRequestPayload): Observable<AppUser> {
        //return this.http.post<AppUser>(`${this.baseUrl}/login`, authRequest);
        console.log('Logging in with username', authRequest);
        return of(createRandomAppUser());
    }

    signup(authRequest: AuthRequestPayload): Observable<AppUser> {
        //return this.http.post<AppUser>(`${this.baseUrl}/signup`, authRequest);
        console.log('Signing up with username', authRequest);
        return of(createRandomAppUser());
    }
}