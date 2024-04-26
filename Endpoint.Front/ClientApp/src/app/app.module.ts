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
import { AddCountryComponent } from './countries/addCountry.component';
import { EditCountryComponent } from './countries/editCountry.component';
import { ProvincesComponent } from './provinces/provinces.component';
import { AddProvinceComponent } from './provinces/addProvince.component';
import { EditProvinceComponent } from './provinces/editProvince.component';
import { JobCategoriesComponent } from './jobCategories/jobCategories.component';
import { AddJobCategoryComponent } from './jobCategories/addJobCategory.component';
import { EditJobCategoryComponent } from './jobCategories/editJobCategory.component';
import { LanguagesComponent } from './languages/languages.component';
import { AddLanguageComponent } from './languages/addLanguage.component';
import { EditLanguageComponent } from './languages/editLanguage.component';
import { MethodOfRecieveQuestionsComponent } from './methodOfRecieveQuestions/methodOfRecieveQuestions.component';
import { AddMethodOfRecieveQuestionComponent } from './methodOfRecieveQuestions/addMethodOfRecieveQuestion.component';
import { EditMethodOfRecieveQuestionComponent } from './methodOfRecieveQuestions/editMethodOfRecieveQuestion.component';
import { QuestionTreeComponent } from './questionCategories/questionTree.component';
import { QuestionCategoriesComponent } from './questionCategories/questionCategories.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    AdminsComponent,
    AddAdminComponent,
    AdminAccessComponent,
    CountriesComponent,
    AddCountryComponent,
    EditCountryComponent,
    ProvincesComponent,
    AddProvinceComponent,
    EditProvinceComponent,
    JobCategoriesComponent,
    AddJobCategoryComponent,
    EditJobCategoryComponent,
    LanguagesComponent,
    AddLanguageComponent,
    EditLanguageComponent,
    MethodOfRecieveQuestionsComponent,
    AddMethodOfRecieveQuestionComponent,
    EditMethodOfRecieveQuestionComponent,
    QuestionCategoriesComponent,
    QuestionTreeComponent
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
      { path: 'addCountry', component: AddCountryComponent, canActivate: [AuthGuard] },
      { path: 'editCountry/:id', component: EditCountryComponent, canActivate: [AuthGuard] },
      { path: 'provinces', component: ProvincesComponent, canActivate: [AuthGuard] },
      { path: 'addProvince', component: AddProvinceComponent, canActivate: [AuthGuard] },
      { path: 'editProvince/:id', component: EditProvinceComponent, canActivate: [AuthGuard] },
      { path: 'jobCategories', component: JobCategoriesComponent, canActivate: [AuthGuard] },
      { path: 'addJobCategory', component: AddJobCategoryComponent, canActivate: [AuthGuard] },
      { path: 'editJobCategory/:id', component: EditJobCategoryComponent, canActivate: [AuthGuard] },
      { path: 'languages', component: LanguagesComponent, canActivate: [AuthGuard] },
      { path: 'addLanguage', component: AddLanguageComponent, canActivate: [AuthGuard] },
      { path: 'editLanguage/:id', component: EditLanguageComponent, canActivate: [AuthGuard] },
      { path: 'methodOfRecieveQuestions', component: MethodOfRecieveQuestionsComponent, canActivate: [AuthGuard] },
      { path: 'addMethodOfRecieveQuestion', component: AddMethodOfRecieveQuestionComponent, canActivate: [AuthGuard] },
      { path: 'editMethodOfRecieveQuestion/:id', component: EditMethodOfRecieveQuestionComponent, canActivate: [AuthGuard] },
      { path: 'questionCats', component: QuestionCategoriesComponent, canActivate: [AuthGuard] },
    ])
  ],
  providers: [StoreService],
  bootstrap: [AppComponent]
})
export class AppModule { }
