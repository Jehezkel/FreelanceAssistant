import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GetProjectsInput } from '../_models/get-projects-input.model';

@Injectable({
  providedIn: 'root',
})
export class ApiClientService {
  API_BASE_URL: string = environment.API_BASE_PATH;
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
  activate(tokenValue: string) {
    const uri = this.API_BASE_URL + 'Account/ActivateAccount';
    const params = new HttpParams().set('tokenValue', tokenValue);
    return this.httpClient.get(uri, { params: params });
  }

  fl_GetIntegrationStatus() {
    const uri = this.API_BASE_URL + 'Freelance';
    return this.httpClient.get(uri);
  }
  fl_VerifyCode(code: string) {
    const uri = this.API_BASE_URL + 'Freelance/VerifyCode';
    const params = new HttpParams().set('code', code);
    return this.httpClient.get(uri, { params: params });
  }
  fl_GetProjects(input: GetProjectsInput) {
    const uri = this.API_BASE_URL + 'Freelance/Projects';
  }
}
