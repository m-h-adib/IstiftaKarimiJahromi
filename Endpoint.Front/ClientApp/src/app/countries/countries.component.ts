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

@Component({
  templateUrl: './countries.component.html',
  selector: 'CountriesComponent'
})
export class CountriesComponent implements OnInit {
  deleteDialog: boolean = false;
  isAccess: boolean = false;
  countries?: ListDataResultDto<GetCountryDto>;
  selectCountry?: number;

  access: string[] = [];

  constructor(private _getCountriesService: GetCountriesService,
    private _deleteCountryService: DeleteCountryService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.activatedRoute.queryParams.subscribe((params: Params): void => {
     
      //this.req.name = params.name;
      //this.req.username = params.username;
      if (this.access.find(p => p == "Countries") != undefined) {
        this.isAccess = true;
        this.getData();
      }
     // console.log(this.req);
    });

    
  }



  getData() {
    if (this.isAccess) {
      this._getCountriesService.GetUsers().subscribe(p => {
        this.countries = p;
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
    this.selectCountry = id;
    this.deleteDialog = !this.deleteDialog;
  }

  deleteCountry() {
    this._deleteCountryService.DeleteCountry(this.selectCountry!).subscribe(p => {
      if (p.isSuccess) {
        this.countries!.data = this.countries!.data!.filter(p => p.id != this.selectCountry);
        this.showDeleteDialog();
        
      }
    })
  }
}
