import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Route,
  Router,
} from '@angular/router';
import { finalize } from 'rxjs';
import { MessagesService } from '../_services/messages.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  sourceUrl: string = '';
  loginForm: FormGroup;
  isLoading: boolean = false;
  constructor(
    route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    fb: FormBuilder,
    private msgService: MessagesService
  ) {
    this.loginForm = fb.group({
      email: ['', [Validators.email, Validators.required]],
      pass: ['', Validators.required],
    });
    route.queryParamMap.subscribe(
      (p) => (this.sourceUrl = p.get('redirect') ?? '')
    );
  }

  ngOnInit(): void {
    if (this.userService.isLoggedInValue()) {
      this.router.navigateByUrl('/');
    }
  }
  submitLogin() {
    this.isLoading = true;
    this.userService
      .login(
        this.loginForm.controls.email.value,
        this.loginForm.controls.pass.value
      )
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.router.navigateByUrl(this.sourceUrl);
          this.msgService.addSuccess(
            'Welcome, ' + this.userService.User$.value.userName
          );
        },
        error: (error) => this.msgService.addError(error.message, 10),
      });
  }
}
