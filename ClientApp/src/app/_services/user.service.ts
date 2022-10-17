import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { ApiClientService } from './api-client.service';

class User {
  userName?: string;
  accessToken?: string;
  refreshToken?: string;
  expireDate?: string;
  userRoles?: string[] = [];
}
// "accessToken": "string",
// "refreshToken": "string",
// "expireDate": "2022-10-17T17:23:58.578Z",
// "userName": "string",
// "userRoles": [
//   "string"
// ]
@Injectable({
  providedIn: 'root',
})
export class UserService {
  User$: BehaviorSubject<User> = new BehaviorSubject<User>({});

  constructor(private apiService: ApiClientService) {
    console.log('created user service');
    this.retriveUser();
  }
  login(email: string, pass: string) {
    return this.apiService.login(email, pass).pipe(
      tap((data: User) => {
        this.User$.next(data);
        this.saveUser();
      })
    );
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
