import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { MomentModule } from 'ngx-moment';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { ActivateComponent } from './account/activate/activate.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './helpers/auth.guard';
import { environment } from 'src/environments/environment';
import { MessagesComponent } from './messages/messages.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    MessagesComponent,
    ActivateComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MomentModule,
  ],
  providers: [
    AuthGuard,
    // { provide: 'API_BASE_URL', use: environment.API_BASE_PATH }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
