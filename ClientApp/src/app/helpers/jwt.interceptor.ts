import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserService } from '@services/user.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  API_BASE_URL = environment.API_BASE_PATH;
  constructor(private userService: UserService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const isApiUrl = request.url.startsWith(this.API_BASE_URL);
    const isLoggedIn = this.userService.isLoggedIn;
    if (isApiUrl && isLoggedIn) {
      const tokenVal = this.userService.accessToken;
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${tokenVal}` },
      });
    }
    return next.handle(request);
  }
}
