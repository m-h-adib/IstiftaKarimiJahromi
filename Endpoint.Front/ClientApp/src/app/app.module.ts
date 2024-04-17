import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
// for HttpClient import:
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
// for Router import:
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
// for Core import:
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { StoreService } from './StoreService';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { AdminsComponent } from './users/admins.component';
import { AddAdminComponent } from './users/addAdmin.component';
import { AdminAccessComponent } from './users/adminAccess.component';
import { CountriesComponent } from './countries/countries.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    AdminsComponent,
    AddAdminComponent,
    AdminAccessComponent,
    CountriesComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    // for HttpClient use:
    LoadingBarHttpClientModule,
    // for Router use:
    LoadingBarRouterModule,
    // for Core use:
    LoadingBarModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent },
      { path: 'users', component: AdminsComponent, canActivate: [AuthGuard] },
      { path: 'adduser', component: AddAdminComponent, canActivate: [AuthGuard] },
      { path: 'useraccess/:id', component: AdminAccessComponent, canActivate: [AuthGuard] },
      { path: 'countries', component: CountriesComponent, canActivate: [AuthGuard] },
    ])
  ],
  providers: [StoreService],
  bootstrap: [AppComponent]
})
export class AppModule { }
