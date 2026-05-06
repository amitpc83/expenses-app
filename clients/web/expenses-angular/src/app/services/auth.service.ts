import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AuthResponse } from '../models/authresponse';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl + 'Auth';
  private currentUserSubject = new BehaviorSubject<string | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private htttp: HttpClient,
    private router: Router,
  ) {
    const token = localStorage.getItem('token');
    this.currentUserSubject.next(token ? 'user' : null);
  }

  login(credentials: User): Observable<AuthResponse> {
    return this.htttp.post<AuthResponse>(this.apiUrl + '/login', credentials).pipe(
      tap((response) => {
        localStorage.setItem('token', response.token);
        this.currentUserSubject.next('user');
      }),
    );
  }
  register(credentials: User): Observable<AuthResponse> {
    return this.htttp.post<AuthResponse>(this.apiUrl + '/register', credentials).pipe(
      tap((response) => {
        localStorage.setItem('token', response.token);
        this.currentUserSubject.next('user');
      }),
    );
  }
  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  // Here, !! is shortcut for checking true or false
  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
