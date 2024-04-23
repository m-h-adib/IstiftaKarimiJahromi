import { Component, OnInit } from "@angular/core";
import { AddCountryDto } from "../_models/country/AddCountryDto";
import { AddCountryService } from "../_services/country/addCountryService";
import { Router } from "@angular/router";
import { AddJobCategoryDto } from "../_models/jobCategory/AddJobCategoryDto";
import { AddJobCategoryService } from "../_services/jobCategory/addJobCategoryService";

@Component({
  templateUrl: './addJobCategory.component.html',
  selector:'AddJobCategoryComponent'
})
export class AddJobCategoryComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: AddJobCategoryDto = {};
  IsLoading: boolean = false;

  constructor(private _addJobCategoryService: AddJobCategoryService, private router: Router) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "AddJobCategory") == undefined)
      this.router.navigate(["/jobCategories"]);
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان شغلی را وارد نمایید");

    this._addJobCategoryService.AddJobCategory(this.AddDto).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(['/jobCategories']);
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
