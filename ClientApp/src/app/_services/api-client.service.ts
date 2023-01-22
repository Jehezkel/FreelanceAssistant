import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ProjectSearch } from '../_models/project-search';
import { Job } from '../_models/job.model';
import { BidTemplate } from '../_models/bid-template';
import { User } from '../_models/user.model';

@Injectable({
  providedIn: 'root',
})
export class ApiClientService {
  API_BASE_URL: string = environment.API_BASE_PATH;
  PROJ_SEARCH_URL: string = this.API_BASE_URL + 'ProjectSearch';
  BID_TEMPLATE_URL: string = this.API_BASE_URL + 'DescriptionTemplate';
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
  fl_GetProjects(input: ProjectSearch) {
    const uri = this.API_BASE_URL + 'Freelance/Projects';
  }
  fl_getJobs() {
    const uri = this.API_BASE_URL + 'Freelance/GetJobs';
    return this.httpClient.get<Job[]>(uri);
  }
  // templates
  getTemplates() {
    return this.httpClient.get<BidTemplate[]>(this.BID_TEMPLATE_URL);
  }
  addTemplate(desc: string) {
    const reqBody = {
      description: desc,
    };
    return this.httpClient.post<BidTemplate>(this.BID_TEMPLATE_URL, reqBody);
  }
  removeTemplate(id: number) {
    return this.httpClient.delete(`${this.BID_TEMPLATE_URL}/${id}`);
  }
  updateTemplate(template: BidTemplate) {
    return this.httpClient.put<BidTemplate>(this.BID_TEMPLATE_URL, template);
  }

  ///searches

  getSearches() {
    return this.httpClient.get<ProjectSearch[]>(this.PROJ_SEARCH_URL);
  }
  addSearch(projSearch: ProjectSearch) {
    return this.httpClient.post<ProjectSearch>(
      this.PROJ_SEARCH_URL,
      projSearch
    );
  }
  updateSearch(projSearch: ProjectSearch) {
    return this.httpClient.put<ProjectSearch>(this.PROJ_SEARCH_URL, projSearch);
  }
  removeSearch(id: number) {
    return this.httpClient.delete(`${this.PROJ_SEARCH_URL}/${id}`);
  }
}
