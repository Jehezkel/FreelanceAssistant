import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  concatMap,
  filter,
  finalize,
  map,
  of,
  switchMap,
  take,
  tap,
  timeout,
} from 'rxjs';
import { ApiClientService } from 'src/app/_services/api-client.service';
import { MessagesService } from 'src/app/_services/messages.service';

@Component({
  selector: 'app-activate',
  templateUrl: './activate.component.html',
  styleUrls: ['./activate.component.css'],
})
export class ActivateComponent implements OnInit {
  isLoading: boolean = true;
  isTokenEmpty: boolean = true;
  isSuccess: boolean = false;
  code: string = '';
  constructor(
    private route: ActivatedRoute,
    private apiService: ApiClientService,
    private router: Router,
    private msgService: MessagesService
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(
        switchMap((params) => {
          var tokenVal = params.get('tokenValue');
          if (!tokenVal) {
            throw Error('empty token value');
          } else {
            return this.apiService.activate(tokenVal);
          }
        }),
        take(1),
        finalize(() =>
          setTimeout(() => this.router.navigateByUrl('/login'), 1000)
        )
      )
      .subscribe({
        next: (resp) => {
          this.msgService.addSuccess('Account activated!');
          this.isLoading = false;
        },
        error: (err) =>
          this.msgService.addError('Error occured on code verification', 10),
      });
  }
}
