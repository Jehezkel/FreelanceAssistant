import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, of, tap } from 'rxjs';
import { User } from '../_models/user.model';
import { ApiClientService } from './api-client.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  User$: BehaviorSubject<User> = new BehaviorSubject<User>({} as User);
  isLoggedIn$: Observable<boolean>;
  constructor(private apiService: ApiClientService) {
    console.log('created user service');
    this.retriveUser();
    this.isLoggedIn$ = this.User$.pipe(
      map((user: User) => (this.accessToken ? true : false))
    );
  }
  login(email: string, pass: string) {
    // return this.apiService.login(email,pass).pipe(
    //   tap((data : User))
    //   )
    return this.apiService.login(email, pass).pipe(
      tap((data: User) => {
        this.User$.next(data);
        this.saveUser();
      })
    );
  }
  logout() {
    this.User$.next({} as User);
    this.removeUser();
  }
  get isLoggedIn(): boolean {
    return this.User$.value.accessToken ? true : false;
  }
  // get isLoggedIn$(): Observable<boolean> {
  //   return of(this.isLoggedIn);
  // }
  get accessToken(): string | undefined {
    return this.User$.value.accessToken;
  }
  saveUser() {
    window.sessionStorage.setItem('USER', JSON.stringify(this.User$.value));
  }
  removeUser() {
    window.sessionStorage.removeItem('USER');
  }
  retriveUser() {
    const sessionVal = window.sessionStorage.getItem('USER');
    if (sessionVal !== null) {
      const receivedUser = JSON.parse(sessionVal);
      this.User$.next(receivedUser);
    }
  }
}
