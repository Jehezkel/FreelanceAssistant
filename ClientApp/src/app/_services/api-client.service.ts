import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ApiClientService {
  API_BASE_URL: string = 'https://localhost:7195/';
  constructor(private httpClient: HttpClient) {}
  login(email: string, password: string) {
    const uri = this.API_BASE_URL + 'Account/Login';
    const reqBody = {
      email: email,
      password: password,
    };
    return this.httpClient.post(uri, reqBody);
  }
  register(userName: string, email: string, password: string) {
    const uri = this.API_BASE_URL + 'Account/Register';
    const reqBody = {
      email: email,
      password: password,
      userName: userName,
    };
    return this.httpClient.post(uri, reqBody);
  }
}
