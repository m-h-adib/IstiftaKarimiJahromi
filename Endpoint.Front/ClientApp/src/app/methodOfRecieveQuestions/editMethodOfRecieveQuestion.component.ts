import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { UpdateCountryDto } from "../_models/country/UpdateCountryDto";
import { UpdateCountryService } from "../_services/country/updateCountryService";
import { GetOneCountryService } from "../_services/country/getOneCountryService";
import { UpdateJobCategoryDto } from "../_models/jobCategory/UpdateJobCategoryDto";
import { UpdateJobCategoryService } from "../_services/jobCategory/updateJobCategoryService";
import { GetOneJobCategoryService } from "../_services/jobCategory/getOneJobCategoryService";
import { UpdateLanguageDto } from "../_models/language/UpdateLanguageDto";
import { UpdateLanguageService } from "../_services/language/updateLanguageService";
import { GetOneLanguageService } from "../_services/language/getOneLanguageService";
import { UpdateMethodOfRecieveQuestionDto } from "../_models/methodRecieveQuestion/UpdateMethodOfRecieveQuestionDto";
import { UpdateMethodOfRecieveQuestionService } from "../_services/methodOfRecieveQuestion/updateMethodOfRecieveQuestionService";
import { GetOneMethodOfRecieveQuestionService } from "../_services/methodOfRecieveQuestion/getOneMethodOfRecieveQuestionService";

@Component({
  templateUrl: './editMethodOfRecieveQuestion.component.html',
  selector:'EditMethodOfRecieveQuestionComponent'
})
export class EditMethodOfRecieveQuestionComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: UpdateMethodOfRecieveQuestionDto = {};
  IsLoading: boolean = false;
  id?: string = undefined;

  constructor(private _updateMethodOfRecieveQuestionService: UpdateMethodOfRecieveQuestionService
    , private router: Router
    , private route: ActivatedRoute
    , private _getOneMethodOfRecieveQuestionService: GetOneMethodOfRecieveQuestionService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "UpdateMethodOfRecieveQuestion") == undefined)
      this.router.navigate(["/methodOfRecieveQuestions"]);

    this.id = this.route.snapshot.paramMap.get('id')!;


    this._getOneMethodOfRecieveQuestionService.GetOneMethodOfRecieveQuestion(Number.parseInt(this.id)).subscribe(p => {
      if (p.isSuccess)
        this.AddDto = p.data;
      else
        this.Errors.push(p.message);
    })
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان را وارد نمایید");

    this._updateMethodOfRecieveQuestionService.UpdateMethodOfRecieveQuestion(this.AddDto).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(['/methodOfRecieveQuestions']);
      }
      else {
        for (let a of p.message.split('|'))
          this.Errors.push(a);
      }
    }, (error) => {

      for (let a of error.error.message.split('|'))
        this.Errors.push(a);
    })
  }
}
