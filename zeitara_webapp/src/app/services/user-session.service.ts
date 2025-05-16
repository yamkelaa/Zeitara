import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../shared/models/user/user';

const USER_STORAGE_KEY = 'currentUser';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser$: Observable<User | null>; // declare only here

  constructor() {
    const storedUserJson = localStorage.getItem('currentUser');
    const storedUser = storedUserJson ? JSON.parse(storedUserJson) as User : null;

    this.currentUserSubject = new BehaviorSubject<User | null>(storedUser);
    this.currentUser$ = this.currentUserSubject.asObservable(); // ✅ now safe
  }

  setCurrentUser(user: User): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  clearCurrentUser(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}

