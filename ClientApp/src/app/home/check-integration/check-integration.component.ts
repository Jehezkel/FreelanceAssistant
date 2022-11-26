import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '@services/api-client.service';
import { Observable } from 'rxjs';
import { finalize, startWith, tap } from 'rxjs/operators';
import { HttpRequestState } from 'src/app/helpers/HttpRequestState';

@Component({
  selector: 'app-check-integration',
  templateUrl: './check-integration.component.html',
  styleUrls: ['./check-integration.component.css'],
})
export class CheckIntegrationComponent implements OnInit {
  isLoading: boolean = false;
  authUrl: string = '';
  isConfigured: boolean = false;
  // readonly integrationState$:Observable<HttpRequestState<>>
  constructor(private apiClient: ApiClientService) {}

  ngOnInit(): void {
    this.apiClient
      .fl_GetIntegrationStatus()
      .pipe(
        startWith((this.isLoading = true)),
        finalize(() => (this.isLoading = false))
      )
      .subscribe((data: any) => {
        if (data != null && !data.authUrl) {
          this.isConfigured = true;
          this.authUrl = data.authUrl ?? '';
        }
      });
  }
}
