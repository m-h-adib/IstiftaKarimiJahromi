import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { GetUsersService } from "../_services/user/getUsersService";
import { ListDataResultDto, PagingDataResultDto } from "../_models/common/ResultDto";
import { GetUsersDto } from "../_models/user/GetUsersDto";
import { GetUsersRequestDto } from "../_models/user/GetUsersRequestDto";
import { DeleteUserService } from "../_services/user/DeleteUserService";
import { GetCountryDto } from "../_models/country/GetCountryDto";
import { DeleteCountryService } from "../_services/country/deleteCountryService";
import { GetCountriesService } from "../_services/country/getCountriesService";
import { GetJobCategoryDto } from "../_models/jobCategory/GetJobCategoryDto";
import { GetJobCategoriesService } from "../_services/jobCategory/getJobCategoriesService";
import { DeleteJobCategoryService } from "../_services/jobCategory/deleteJobCategoryService";
import { GetLanguageDto } from "../_models/language/GetLanguageDto";
import { GetLanguagesService } from "../_services/language/getLanguagesService";
import { DeleteLanguageService } from "../_services/language/deleteLanguageService";
import { GetMethodOfRecieveQuestionsService } from "../_services/methodOfRecieveQuestion/getMethodOfRecieveQuestionsService";
import { DeleteMethodOfRecieveQuestionService } from "../_services/methodOfRecieveQuestion/deleteMethodOfRecieveQuestionService";
import { GetMethodOfRecieveQuestionDto } from "../_models/methodRecieveQuestion/GetMethodOfRecieveQuestionDto";

@Component({
  templateUrl: './methodOfRecieveQuestions.component.html',
  selector: 'MethodOfRecieveQuestionsComponent'
})
export class MethodOfRecieveQuestionsComponent implements OnInit {
  deleteDialog: boolean = false;
  isAccess: boolean = false;
  items?: ListDataResultDto<GetMethodOfRecieveQuestionDto>;
  selectItem?: number;

  access: string[] = [];

  constructor(private _getMethodOfRecieveQuestionsService: GetMethodOfRecieveQuestionsService,
    private _deleteMethodOfRecieveQuestionService: DeleteMethodOfRecieveQuestionService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.activatedRoute.queryParams.subscribe((params: Params): void => {
     
      //this.req.name = params.name;
      //this.req.username = params.username;
      if (this.access.find(p => p == "MethodOfRecieveQuestions") != undefined) {
        this.isAccess = true;
        this.getData();
      }
     // console.log(this.req);
    });

    
  }



  getData() {
    if (this.isAccess) {
      this._getMethodOfRecieveQuestionsService.GetMethodOfRecieveQuestions().subscribe(p => {
        this.items = p;
      })
    }
  }

  getAccess(id: string): boolean {
    if (this.access.find(p => p == id) != undefined)
      return true;
    else
      return false;
  }

  showDeleteDialog(id?: number) {
    this.selectItem = id;
    this.deleteDialog = !this.deleteDialog;
  }

  deleteMethodOfRecieveQuestion() {
    this._deleteMethodOfRecieveQuestionService.DeleteMethodOfRecieveQuestion(this.selectItem!).subscribe(p => {
      if (p.isSuccess) {
        this.items!.data = this.items!.data!.filter(p => p.id != this.selectItem);
        this.showDeleteDialog();
        
      }
    })
  }
}
