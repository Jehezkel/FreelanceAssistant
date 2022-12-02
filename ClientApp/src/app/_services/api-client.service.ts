import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetProjectsInput } from '../_models/get-projects-input.model';
import { Job } from '../_models/job.model';
import { Template } from '../_models/template.model';
import { User } from '../_models/user.model';

@Injectable({
  providedIn: 'root',
})
export class ApiClientService {
  API_BASE_URL: string = environment.API_BASE_PATH;
  constructor(private httpClient: HttpClient) {}
  login(email: string, password: string): Observable<User> {
    const uri = this.API_BASE_URL + 'Account/Login';
    const reqBody = {
      email: email,
      password: password,
    };
    return this.httpClient.post<User>(uri, reqBody);
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
  fl_getJobs() {
    const uri = this.API_BASE_URL + 'Freelance/GetJobs';
    return this.httpClient.get<Job[]>(uri);
  }
  getTemplates() {
    const uri = this.API_BASE_URL + 'DescriptionTemplate';
    return this.httpClient.get<Template[]>(uri);
  }
  addTemplate(desc: string) {
    const uri = this.API_BASE_URL + 'DescriptionTemplate';
    const reqBody = {
      description: desc,
    };
    return this.httpClient.post<Template>(uri, reqBody);
  }
  removeTemplate(id: number) {
    const uri = this.API_BASE_URL + `DescriptionTemplate/${id}`;
    return this.httpClient.delete(uri);
  }
  updateTemplate(template: Template) {
    const uri = this.API_BASE_URL + 'DescriptionTemplate';
    return this.httpClient.put<Template>(uri, template);
  }
}
