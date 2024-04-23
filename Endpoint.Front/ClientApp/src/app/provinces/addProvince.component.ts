import { Component, OnInit } from "@angular/core";
import { AddCountryDto } from "../_models/country/AddCountryDto";
import { AddCountryService } from "../_services/country/addCountryService";
import { Router } from "@angular/router";
import { AddProvinceDto } from "../_models/province/AddProvinceDto";
import { AddProvinceService } from "../_services/province/addProvinceService";
import { GetCountryDto } from "../_models/country/GetCountryDto";
import { GetCountriesService } from "../_services/country/getCountriesService";

@Component({
  templateUrl: './addProvince.component.html',
  selector:'AddProvinceComponent'
})
export class AddProvinceComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: AddProvinceDto = {};
  IsLoading: boolean = false;
  countries: GetCountryDto[] = [];

  constructor(private _addProvinceService: AddProvinceService
    , private router: Router
    ,private _getCountriesService: GetCountriesService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "AddProvince") == undefined)
      this.router.navigate(["/provinces"]);

    this._getCountriesService.GetUsers().subscribe(p => {
      this.countries = p.data;
    })
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان استان را وارد نمایید");

    if (this.AddDto.countryId == null)
      this.Errors.push("لطفا کشور را انتخاب نمایید");

    this._addProvinceService.AddProvince(this.AddDto).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(['/provinces']);
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
