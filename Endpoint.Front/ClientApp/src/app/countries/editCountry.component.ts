import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { UpdateCountryDto } from "../_models/country/UpdateCountryDto";
import { UpdateCountryService } from "../_services/country/updateCountryService";
import { GetOneCountryService } from "../_services/country/getOneCountryService";

@Component({
  templateUrl: './editCountry.component.html',
  selector:'EditCountryComponent'
})
export class EditCountryComponent implements OnInit {
  Errors: string[] = [];
  access: string[] = [];
  AddDto: UpdateCountryDto = {};
  IsLoading: boolean = false;
  id?: string = undefined;

  constructor(private _updateCountryService: UpdateCountryService
    , private router: Router
    , private route: ActivatedRoute
    , private _getOneCountryService: GetOneCountryService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "UpdateCountry") == undefined)
      this.router.navigate(["/countries"]);

    this.id = this.route.snapshot.paramMap.get('id')!;


    this._getOneCountryService.GetOneCountry(Number.parseInt(this.id)).subscribe(p => {
      if (p.isSuccess)
        this.AddDto = p.data;
      else
        this.Errors.push(p.message);
    })
  }

  Save() {
    if (this.AddDto.name == null || this.AddDto.name == "")
      this.Errors.push("لطفا عنوان کشور را وارد نمایید");

    this._updateCountryService.UpdateCountry(this.AddDto).subscribe(p => {
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
