import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { GetUsersService } from "../_services/user/getUsersService";
import { ListDataResultDto, PagingDataResultDto } from "../_models/common/ResultDto";
import { GetUsersDto } from "../_models/user/GetUsersDto";
import { GetUsersRequestDto } from "../_models/user/GetUsersRequestDto";
import { DeleteUserService } from "../_services/user/DeleteUserService";
import { GetCountryDto } from "../_models/country/GetCountryDto";
import { DeleteCountryService } from "../_services/country/deleteCountryService";
import { GetCountriesService } from "../_services/country/getCountriesService";
import { GetProvinceDto } from "../_models/province/GetProvinceDto";
import { GetProvincesService } from "../_services/province/getProvincesService";
import { DeleteProvinceService } from "../_services/province/deleteProvinceService";
import { count } from "rxjs";

@Component({
  templateUrl: './provinces.component.html',
  selector: 'ProvincesComponent'
})
export class ProvincesComponent implements OnInit {
  deleteDialog: boolean = false;
  isAccess: boolean = false;
  items?: ListDataResultDto<GetProvinceDto>;
  selectItem?: number;
  countryId?: number = undefined;
  countries: GetCountryDto[] = [];

  access: string[] = [];

  constructor(private _getProvincesService: GetProvincesService,
    private _deleteProvinceService: DeleteProvinceService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private _getCountriesService: GetCountriesService) { }


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.activatedRoute.queryParams.subscribe((params: Params): void => {
     
      //this.req.name = params.name;
      //this.req.username = params.username;
      if (this.access.find(p => p == "Provinces") != undefined) {
        this.isAccess = true;
        this.getData();
      }
     // console.log(this.req);
    });


    this._getCountriesService.GetUsers().subscribe(p => {
      if (p.isSuccess)
        this.countries = p.data;
    });

  }



  getData() {
    if (this.isAccess) {
      this._getProvincesService.GetProvinces(this.countryId).subscribe(p => {
        this.items = p;
      })
    }
  }

  getAccess(id: string): boolean {
    if (this.access.find(p => p == id) != undefined)
      return true;
    else
      return false;
  }

  showDeleteDialog(id?: number) {
    this.selectItem = id;
    this.deleteDialog = !this.deleteDialog;
  }

  deleteProvince() {
    this._deleteProvinceService.DeleteProvince(this.selectItem!).subscribe(p => {
      if (p.isSuccess) {
        this.items!.data = this.items!.data!.filter(p => p.id != this.selectItem);
        this.showDeleteDialog();
        
      }
    })
  }
}
