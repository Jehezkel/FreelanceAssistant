import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './helpers/auth.guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { ActivateComponent } from './account/activate/activate.component';
import { VerifyCodeComponent } from './home/verify-code/verify-code.component';
import { TemplatesComponent } from './bid-templates/templates.component';
import { BidsComponent } from './bids/bids.component';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectSearchesComponent } from './project-searches/project-searches.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'activate', component: ActivateComponent },
  { path: 'VerifyCode', component: VerifyCodeComponent },
  { path: 'bid-templates', component: TemplatesComponent },
  { path: 'bids', component: BidsComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'project-searches', component: ProjectSearchesComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
