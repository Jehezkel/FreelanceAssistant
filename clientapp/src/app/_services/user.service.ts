import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

class User {
  name?: string;
  accessToken?: string;
  refreshToken?: string;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  User$: BehaviorSubject<User> = new BehaviorSubject<User>({});

  constructor() {
    console.log('created user service');
    this.retriveUser();
  }
  login() {
    this.User$.next({
      name: 'Janusz',
      accessToken: 'xDDDDDDDDDDDDDDDDDDDDD',
      refreshToken: 'dddddddddddddddddddddxxxxxxxxxxxxxxx',
    } as User);
    this.saveUser();
  }
  logout() {
    this.User$.next(new User());
  }
  isLoggedInValue() {
    return this.User$.value.accessToken ? true : false;
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
