import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private platformId = inject(PLATFORM_ID);

  private apiUrl = 'https://localhost:44399/api/auth';

  private loggedIn = new BehaviorSubject<boolean>(
    this.hasCredentials()
  );

  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient) {}

  hasCredentials(): boolean {

    if (isPlatformBrowser(this.platformId)) {
      return !!localStorage.getItem('auth_credentials');
    }

    return false;
  }

  login(username: string, password: string): Observable<any> {

    return this.http
      .post(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(() => {

          if (isPlatformBrowser(this.platformId)) {

            const credentials = btoa(`${username}:${password}`);

            localStorage.setItem('auth_credentials', credentials);
            localStorage.setItem('auth_user', username);
          }

          this.loggedIn.next(true);
        })
      );
  }

  logout(): void {

    if (isPlatformBrowser(this.platformId)) {

      localStorage.removeItem('auth_credentials');
      localStorage.removeItem('auth_user');
    }

    this.loggedIn.next(false);
  }

  getAuthHeaders(): HttpHeaders {

    let credentials = '';

    if (isPlatformBrowser(this.platformId)) {
      credentials = localStorage.getItem('auth_credentials') || '';
    }

    return new HttpHeaders({
      Authorization: `Basic ${credentials}`
    });
  }

  getUsername(): string {

    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('auth_user') || '';
    }

    return '';
  }

  isLoggedIn(): boolean {
    return this.hasCredentials();
  }
}