import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, finalize, map, switchMap, tap, timeout } from 'rxjs';
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
  code: string = '';
  constructor(
    private route: ActivatedRoute,
    private apiService: ApiClientService,
    private router: Router,
    private msgService: MessagesService
  ) {
    // route.queryParamMap.pipe(
    //   map((p) => p.get('tokenValue') ?? ''),
    //   filter((v) => v.length > 1),
    //   tap(() => ((this.isLoading = true), (this.isTokenEmpty = false))),
    //   switchMap((tokenVal) => {
    //     apiService.activate(tokenVal);
    //   })
    // );
  }

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(
        switchMap((params) =>
          this.apiService.activate(params.get('tokenValue') ?? 'asdd')
        ),
        finalize(() => {
          (this.isLoading = false), this.router.navigateByUrl('/login');
        })
      )
      .subscribe({
        next: (response) => {
          this.msgService.addSuccess('Account activated!');
        },
        error: (err) => {
          this.msgService.addError('Error occured on account activation.', 10);
        },
      });
  }
}
