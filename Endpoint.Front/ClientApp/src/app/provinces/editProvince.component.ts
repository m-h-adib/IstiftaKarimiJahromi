import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { UpdateCountryDto } from "../_models/country/UpdateCountryDto";
import { UpdateCountryService } from "../_services/country/updateCountryService";
import { GetOneCountryService } from "../_services/country/getOneCountryService";
import { UpdateProvinceDto } from "../_models/province/UpdateProvinceDto";
import { UpdateProvinceService } from "../_services/province/updateProvinceService";
import { GetOneProvinceService } from "../_services/province/getOneProvinceService";
import { GetCountryDto } from "../_models/country/GetCountryDto";
import { GetCountriesService } from "../_services/country/getCountriesService";

@Component({
  templateUrl: './editProvince.component.html',
  selector:'EditProvinceComponent'
})
export class EditProvinceComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: UpdateProvinceDto = {};
  IsLoading: boolean = false;
  id?: string = undefined;
  countries: GetCountryDto[]= [];


  constructor(private _updateProvinceService: UpdateProvinceService
    , private router: Router
    , private route: ActivatedRoute
    , private _getOneProvinceService: GetOneProvinceService
    , private _getCountriesService: GetCountriesService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "UpdateProvince") == undefined)
      this.router.navigate(["/provinces"]);

    this.id = this.route.snapshot.paramMap.get('id')!;

    this._getCountriesService.GetUsers().subscribe(p => {
      this.countries = p.data;
    })

    this._getOneProvinceService.GetOneProvince(Number.parseInt(this.id)).subscribe(p => {
      if (p.isSuccess)
        this.AddDto = p.data;
      else
        this.Errors.push(p.message);
    })


  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان استان را وارد نمایید");

    if (this.AddDto.countryId == null)
      this.Errors.push("لطفا کشور را انتخاب نمایید");


    this._updateProvinceService.UpdateProvince(this.AddDto).subscribe(p => {
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
