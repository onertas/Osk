import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpApiService } from './http-api-service';
export interface CurrentUser {
  userId: string;
  username: string;
  roles: string[];
}
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private userSubject = new BehaviorSubject<CurrentUser | null>(null);
  user$ = this.userSubject.asObservable();

  constructor(private http: HttpApiService) {}

  loadUser() {
    this.http.get<CurrentUser>('auth/me').subscribe({
      next: (res) => this.userSubject.next(res.data!),
      error: () => this.userSubject.next(null),
    });
  }

  setUser(user: CurrentUser) {
    this.userSubject.next(user);
  }

  clear() {
    this.userSubject.next(null);
  }

  get snapshot() {
    return this.userSubject.value;
  }

  get isLoggedIn() {
    return !!this.userSubject.value;
  }

  hasRole(role: string) {
    return this.userSubject.value?.roles.includes(role) ?? false;
  }
}
