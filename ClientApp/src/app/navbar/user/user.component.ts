import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '@services/user.service';
import { User } from 'src/app/_models/user.model';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {}
  logOut() {
    this.userService.logout();
    this.router.navigateByUrl('/login');
    console.log('logging out');
  }
}
