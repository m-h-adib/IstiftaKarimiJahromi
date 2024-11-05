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
import { QuestionsComponent } from './questions/questions.component';
import { AddQuestionComponent } from './questions/addQuestion.component';
import { NgPersianDatepickerModule } from 'ng-persian-datepicker';
import { EditQuestionComponent } from './questions/editQuestion.component';
import { ProcessQuestionsComponent } from './process/processQuestions.component';
import { ProcessCyclesComponent } from './process/processCycles.component';
import { SearchQuestionComponent } from './process/searchQuestion.component';
import { ResearchCyclesComponent } from './research/researchCycles.component';
import { ResearchQuestionsComponent } from './research/researchQuestions.component';
import { EditorQuestionsComponent } from './editor/editorQuestions.component';
import { EditorCyclesComponent } from './editor/editorCycles.component';
import { ArchivesComponent } from './Archive/archives.component';
import { QuestionDetailComponent } from './Archive/questionDetail.component';
import { QuestionStepsComponent } from './Archive/questionStep.component';
import { QuestionResultComponent } from './Archive/questionResult.component';
import { SessionsComponent } from './sessions/sessions.component';
import { AddSessionComponent } from './sessions/addSession.component';
import { UpdateSessionComponent } from './sessions/updateSession.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { QuestionPopupComponent } from './questions/questionPopup.component';
import { UpdateAdminComponent } from './users/updateAdmin.component';
import { ReportsComponent } from './reports/reports.component';



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
    QuestionTreeComponent,
    QuestionsComponent,
    AddQuestionComponent,
    EditQuestionComponent,
    ProcessQuestionsComponent,
    ProcessCyclesComponent,
    SearchQuestionComponent,
    ResearchCyclesComponent,
    ResearchQuestionsComponent,
    EditorQuestionsComponent,
    EditorCyclesComponent,
    ArchivesComponent,
    QuestionDetailComponent,
    QuestionStepsComponent,
    QuestionResultComponent,
    SessionsComponent,
    AddSessionComponent,
    UpdateSessionComponent,
    QuestionPopupComponent,
    UpdateAdminComponent,
    ReportsComponent
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
    //npm install sweetalert2 @sweetalert2/ngx-sweetalert2@^12.0.0
    SweetAlert2Module.forRoot(),
    NgPersianDatepickerModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent },
      { path: 'users', component: AdminsComponent, canActivate: [AuthGuard] },
      { path: 'adduser', component: AddAdminComponent, canActivate: [AuthGuard] },
      { path: 'useraccess/:id', component: AdminAccessComponent, canActivate: [AuthGuard] },
      { path: 'updateuser/:id', component: UpdateAdminComponent, canActivate: [AuthGuard] },
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
      { path: 'questions', component: QuestionsComponent, canActivate: [AuthGuard] },
      { path: 'addquestion', component: AddQuestionComponent, canActivate: [AuthGuard] },
      { path: 'editquestion/:id', component: EditQuestionComponent, canActivate: [AuthGuard] },
      { path: 'processQuestions', component: ProcessQuestionsComponent, canActivate: [AuthGuard] },
      { path: 'processCycles/:questionId', component: ProcessCyclesComponent, canActivate: [AuthGuard] },
      { path: 'researchQuestions', component: ResearchQuestionsComponent, canActivate: [AuthGuard] },
      { path: 'researchCycles/:questionId', component: ResearchCyclesComponent, canActivate: [AuthGuard] },
      { path: 'editors', component: EditorQuestionsComponent, canActivate: [AuthGuard] },
      { path: 'editorCycles/:questionId', component: EditorCyclesComponent, canActivate: [AuthGuard] },
      { path: 'archives', component: ArchivesComponent, canActivate: [AuthGuard] },
      { path: 'sessions', component: SessionsComponent, canActivate: [AuthGuard] },
      { path: 'addsession/:researchid/:questionid', component: AddSessionComponent, canActivate: [AuthGuard] },
      { path: 'updatesession/:id', component: UpdateSessionComponent, canActivate: [AuthGuard] },
      { path: 'reports', component: ReportsComponent, canActivate: [AuthGuard] }
    ])
  ],
  providers: [StoreService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
