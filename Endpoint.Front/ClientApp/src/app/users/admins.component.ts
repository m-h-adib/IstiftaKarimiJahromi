import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { GetUsersService } from "../_services/user/getUsersService";
import { PagingDataResultDto } from "../_models/common/ResultDto";
import { GetUsersDto } from "../_models/user/GetUsersDto";
import { GetUsersRequestDto } from "../_models/user/GetUsersRequestDto";
import { DeleteUserService } from "../_services/user/DeleteUserService";
import { ResetPasswordService } from "../_services/user/ResetPasswordService";
import { SwalComponent } from "@sweetalert2/ngx-sweetalert2";


@Component({
  templateUrl: './admins.component.html',
  selector: 'AdminsComponent'
})
export class AdminsComponent implements OnInit {
  deleteDialog: boolean = false;
  isAccess: boolean = false;
  pageCount: number = 0;
  mod: number = 0;
  admins?: PagingDataResultDto<GetUsersDto>;
  selectUser?: number;
  req: GetUsersRequestDto = {
    pageNumber: 1,
    pageSize: 10,
    name: "",
    username:""
  };
  access: string[] = [];
  @ViewChild('alertSwal')
  public readonly alertSwal!: SwalComponent;

  constructor(private _getUsersService: GetUsersService,
    private _deleteUserService: DeleteUserService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private resetPass:ResetPasswordService) { }


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.activatedRoute.queryParams.subscribe((params: Params): void => {
     
      //this.req.name = params.name;
      //this.req.username = params.username;
      if (this.access.find(p => p == "Users") != undefined) {
        this.isAccess = true;
        this.getData();
      }
     // console.log(this.req);
    });

    
  }


  resetPassord(userId?:number):void{

    this.resetPass.ResetPassword(userId!).subscribe(p=>{
      this.alertSwal.title=p.message;
      this.alertSwal.text="رمز: "+p.data;
      console.log(p);
      this.alertSwal.fire();
    });

  }

  search(){
    this.req.pageNumber=1;
    this.getData();
  }
  getData() {
    if (this.isAccess) {
      this._getUsersService.GetUsers(this.req).subscribe(p => {
        this.admins = p;
        this.req.pageNumber = p.data!.paging!.page!;
        this.mod = p.data.paging.totalItems / p.data.paging.size;
        this.pageCount = (this.mod == 0 ? Number.parseInt((p.data.paging.totalItems / p.data.paging.size).toString()) : Number.parseInt((p.data.paging.totalItems / p.data.paging.size).toString()) + 1);
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
    this.selectUser = id;
    this.deleteDialog = !this.deleteDialog;
  }

  deleteUser() {
    this._deleteUserService.DeleteUser(this.selectUser!).subscribe(p => {
      if (p.isSuccess) {
        this.admins!.data.items = this.admins!.data!.items.filter(p => p.id != this.selectUser);
        this.showDeleteDialog();
        
      }
    })
  }


  inc() {
   // console.log("inc ");
   // console.log(this.admins);
   // console.log(this.req);
    if (this.req.pageNumber! < this.pageCount) {
      this.req.pageNumber! += 1;
      this.router.navigate([], {
        relativeTo: this.activatedRoute,
        queryParams: this.req,
        queryParamsHandling: 'merge',
      });
    }
  }

  onSearchChange(searchValue: Event): void {
    console.log(searchValue);
  }

  dec() {
    if (this.req.pageNumber != 1) {
      this.req.pageNumber! -= 1;
      this.router.navigate([], {
        relativeTo: this.activatedRoute,
        queryParams: this.req,
        queryParamsHandling: 'merge',
      });


    }
  }

  change(): void {
    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: this.req,
      queryParamsHandling: 'merge',
    });
  }
}
