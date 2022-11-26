import { Component, OnInit } from '@angular/core';
import { UserService } from '@services/user.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  isAuthorized: Observable<boolean>;
  constructor(private userService: UserService) {
    this.isAuthorized=userService.isLoggedIn$;
  }

  ngOnInit(): void {}
}
