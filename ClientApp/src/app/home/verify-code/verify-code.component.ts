import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiClientService } from '@services/api-client.service';
import { MessagesService } from '@services/messages.service';
import { concatMap, filter, finalize, map, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-verify-code',
  templateUrl: './verify-code.component.html',
  styleUrls: ['./verify-code.component.css'],
})
export class VerifyCodeComponent implements OnInit {
  isLoading: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiClientService,
    private router: Router,
    private msgService: MessagesService
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(
        map((params) => {
          var codeVal = params.get('code');
          if (!codeVal) {
            throw new Error('Empty code parameter!');
          } else {
            return codeVal;
          }
        }),
        take(1),
        concatMap((codeVal) => this.apiService.fl_VerifyCode(codeVal)),
        finalize(() => this.router.navigateByUrl('/'))
      )
      .subscribe({
        next: (resp) => {
          this.msgService.addSuccess('Integration succeeded.');
          console.log(resp);
        },
        error: (err) =>
          this.msgService.addError(
            'Error occured on code verification:',
            err.message ?? err
          ),
      });
  }
}
