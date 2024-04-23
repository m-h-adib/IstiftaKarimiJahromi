import { Component, OnInit } from "@angular/core";
import { AddCountryDto } from "../_models/country/AddCountryDto";
import { AddCountryService } from "../_services/country/addCountryService";
import { Router } from "@angular/router";

@Component({
  templateUrl: './addCountry.component.html',
  selector:'AddCountryComponent'
})
export class AddCountryComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: AddCountryDto = {};
  IsLoading: boolean = false;

  constructor(private _addCountryService: AddCountryService, private router: Router) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "AddCountry") == undefined)
      this.router.navigate(["/countries"]);
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان کشور را وارد نمایید");

    this._addCountryService.AddCountry(this.AddDto).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(['/countries']);
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
