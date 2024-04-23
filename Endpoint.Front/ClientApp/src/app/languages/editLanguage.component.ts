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

@Component({
  templateUrl: './editLanguage.component.html',
  selector:'EditLanguageComponent'
})
export class EditLanguageComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: UpdateLanguageDto = {};
  IsLoading: boolean = false;
  id?: string = undefined;

  constructor(private _updateLanguageService: UpdateLanguageService
    , private router: Router
    , private route: ActivatedRoute
    , private _getOneLanguageService: GetOneLanguageService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "UpdateLanguage") == undefined)
      this.router.navigate(["/languages"]);

    this.id = this.route.snapshot.paramMap.get('id')!;


    this._getOneLanguageService.GetOneLanguage(Number.parseInt(this.id)).subscribe(p => {
      if (p.isSuccess)
        this.AddDto = p.data;
      else
        this.Errors.push(p.message);
    })
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان زبان را وارد نمایید");

    this._updateLanguageService.UpdateLanguage(this.AddDto).subscribe(p => {
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
