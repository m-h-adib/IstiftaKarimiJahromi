import { Component, OnInit } from "@angular/core";
import { AddCountryDto } from "../_models/country/AddCountryDto";
import { AddCountryService } from "../_services/country/addCountryService";
import { Router } from "@angular/router";
import { AddJobCategoryDto } from "../_models/jobCategory/AddJobCategoryDto";
import { AddJobCategoryService } from "../_services/jobCategory/addJobCategoryService";
import { AddLanguageDto } from "../_models/language/AddLanguageDto";
import { AddLanguageService } from "../_services/language/addLanguageService";
import { AddMethodOfRecieveQuestionDto } from "../_models/methodRecieveQuestion/AddMethodOfRecieveQuestionDto";
import { AddMethodOfRecieveQuestionService } from "../_services/methodOfRecieveQuestion/addMethodOfRecieveQuestionService";

@Component({
  templateUrl: './addMethodOfRecieveQuestion.component.html',
  selector:'AddMethodOfRecieveQuestionComponent'
})
export class AddMethodOfRecieveQuestionComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: AddMethodOfRecieveQuestionDto = {};
  IsLoading: boolean = false;

  constructor(private _addMethodOfRecieveQuestionService: AddMethodOfRecieveQuestionService, private router: Router) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "AddMethodOfRecieveQuestion") == undefined)
      this.router.navigate(["/methodOfRecieveQuestions"]);
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان را وارد نمایید");

    this._addMethodOfRecieveQuestionService.AddMethodOfRecieveQuestion(this.AddDto).subscribe(p => {
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
