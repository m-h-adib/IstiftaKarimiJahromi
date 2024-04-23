import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { UpdateCountryDto } from "../_models/country/UpdateCountryDto";
import { UpdateCountryService } from "../_services/country/updateCountryService";
import { GetOneCountryService } from "../_services/country/getOneCountryService";
import { UpdateJobCategoryDto } from "../_models/jobCategory/UpdateJobCategoryDto";
import { UpdateJobCategoryService } from "../_services/jobCategory/updateJobCategoryService";
import { GetOneJobCategoryService } from "../_services/jobCategory/getOneJobCategoryService";

@Component({
  templateUrl: './editJobCategory.component.html',
  selector:'EditJobCategoryComponent'
})
export class EditJobCategoryComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: UpdateJobCategoryDto = {};
  IsLoading: boolean = false;
  id?: string = undefined;

  constructor(private _updateJobCategoryService: UpdateJobCategoryService
    , private router: Router
    , private route: ActivatedRoute
    , private _getOneCountryService: GetOneJobCategoryService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "UpdateJobCategory") == undefined)
      this.router.navigate(["/jobCategories"]);

    this.id = this.route.snapshot.paramMap.get('id')!;


    this._getOneCountryService.GetOneJobCategory(Number.parseInt(this.id)).subscribe(p => {
      if (p.isSuccess)
        this.AddDto = p.data;
      else
        this.Errors.push(p.message);
    })
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان شغلی را وارد نمایید");

    this._updateJobCategoryService.UpdateJobCategory(this.AddDto).subscribe(p => {
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
