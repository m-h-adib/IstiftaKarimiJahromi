import { Component, OnInit } from "@angular/core";
import { AddCountryDto } from "../_models/country/AddCountryDto";
import { AddCountryService } from "../_services/country/addCountryService";
import { Router } from "@angular/router";
import { AddJobCategoryDto } from "../_models/jobCategory/AddJobCategoryDto";
import { AddJobCategoryService } from "../_services/jobCategory/addJobCategoryService";
import { AddLanguageDto } from "../_models/language/AddLanguageDto";
import { AddLanguageService } from "../_services/language/addLanguageService";

@Component({
  templateUrl: './addLanguage.component.html',
  selector:'AddLanguageComponent'
})
export class AddLanguageComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: AddLanguageDto = {};
  IsLoading: boolean = false;

  constructor(private _addLanguageService: AddLanguageService, private router: Router) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "AddLanguage") == undefined)
      this.router.navigate(["/languages"]);
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان زبان را وارد نمایید");

    this._addLanguageService.AddLanguage(this.AddDto).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(['/languages']);
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
