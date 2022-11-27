import { Component } from '@angular/core';
import { UserService } from '@services/user.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'clientapp';
  isLoggedIn$: Observable<Boolean>;
  /**
   *
   */
  constructor(private userService: UserService) {
    this.isLoggedIn$ = userService.isLoggedIn$;
  }
}
