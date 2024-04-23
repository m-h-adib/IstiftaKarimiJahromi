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
import { GetJobCategoryDto } from "../_models/jobCategory/GetJobCategoryDto";
import { GetJobCategoriesService } from "../_services/jobCategory/getJobCategoriesService";
import { DeleteJobCategoryService } from "../_services/jobCategory/deleteJobCategoryService";

@Component({
  templateUrl: './jobCategories.component.html',
  selector: 'JobCategoriesComponent'
})
export class JobCategoriesComponent implements OnInit {
  deleteDialog: boolean = false;
  isAccess: boolean = false;
  items?: ListDataResultDto<GetJobCategoryDto>;
  selectItem?: number;

  access: string[] = [];

  constructor(private _getJobCategoriesService: GetJobCategoriesService,
    private _deleteJobCategoryService: DeleteJobCategoryService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.activatedRoute.queryParams.subscribe((params: Params): void => {
     
      //this.req.name = params.name;
      //this.req.username = params.username;
      if (this.access.find(p => p == "JobCategories") != undefined) {
        this.isAccess = true;
        this.getData();
      }
     // console.log(this.req);
    });

    
  }



  getData() {
    if (this.isAccess) {
      this._getJobCategoriesService.GetUsers().subscribe(p => {
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

  deleteJobCategory() {
    this._deleteJobCategoryService.DeleteJobCategory(this.selectItem!).subscribe(p => {
      if (p.isSuccess) {
        this.items!.data = this.items!.data!.filter(p => p.id != this.selectItem);
        this.showDeleteDialog();
        
      }
    })
  }
}
