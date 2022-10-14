import { Component, OnInit } from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Route,
  Router,
} from '@angular/router';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  sourceUrl: string = '';
  constructor(
    route: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) {
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
    this.userService.login();
    this.router.navigateByUrl(this.sourceUrl);
  }
}
