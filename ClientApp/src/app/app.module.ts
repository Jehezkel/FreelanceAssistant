import { NgModule } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';

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
import { ProjectsComponent } from './projects/projects.component';
import { CheckIntegrationComponent } from './home/check-integration/check-integration.component';
import { JwtInterceptor } from './helpers/jwt.interceptor';
import { VerifyCodeComponent } from './home/verify-code/verify-code.component';
import { UserButtonComponent } from './navbar/user-button/user-button.component';
import { NavbarComponent } from './navbar/navbar.component';
import { TemplatesComponent } from './bid-templates/templates.component';
import { BidsComponent } from './bids/bids.component';
import { TemplateFormComponent } from './bid-templates/template-form/template-form.component';
import { JobSelectorComponent } from './projects/job-selector/job-selector.component';
import { ProjectSearchesComponent } from './project-searches/project-searches.component';
import { SearchFormComponent } from './project-searches/search-form/search-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TemplateDataSource } from '@services/template.dataSource';
import { TemplateService } from '@services/bid-template.service';
import { TableComponent } from './_shared/table/table.component';
import { DialogComponent } from './_shared/dialog/dialog.component';
import { TestTableComponent } from './test-table/test-table.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { BidTemplateDialogFormComponent } from './bid-templates/bid-template-dialog-form/bid-template-dialog-form.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    MessagesComponent,
    ActivateComponent,
    ProjectsComponent,
    CheckIntegrationComponent,
    VerifyCodeComponent,
    UserButtonComponent,
    NavbarComponent,
    TemplatesComponent,
    BidsComponent,
    TemplateFormComponent,
    JobSelectorComponent,
    ProjectSearchesComponent,
    SearchFormComponent,
    TableComponent,
    DialogComponent,
    TestTableComponent,
    BidTemplateDialogFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MomentModule,
    BrowserAnimationsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    FormsModule,
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    TemplateDataSource,
    TemplateService,
    // { provide: 'API_BASE_URL', use: environment.API_BASE_PATH }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
