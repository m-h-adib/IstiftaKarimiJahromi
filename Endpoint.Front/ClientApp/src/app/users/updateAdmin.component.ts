import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AddUserDto } from "../_models/user/AddUserDto";
import { AddUserService } from "../_services/user/AddUserService";
import { GetOneUserDto } from "../_models/user/GetOneUserDto";
import { UpdateUserService } from "../_services/user/UpdateUserService";
import { GetOneUserService } from "../_services/user/getOneUserService";

@Component({
  templateUrl: './updateAdmin.component.html',
  selector: 'UpdateAdminComponent'
})
export class UpdateAdminComponent implements OnInit {
  UpdateDto: GetOneUserDto = {
  };
  IsLoading: boolean = false;
  Errors: string[] = [];
  access: string[] = [];

  constructor(private _updateUserService: UpdateUserService,
    private _getOneUserService:GetOneUserService,
    private route:ActivatedRoute,
    private router: Router) { }


  ngOnInit(): void {
    this.UpdateDto.userId = Number.parseInt(this.route.snapshot.paramMap.get('id')!);
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "AddUser") == undefined)
      this.router.navigate(["/users"]);

    this._getOneUserService.GetUsers(this.UpdateDto.userId).subscribe(p=>{
      this.UpdateDto=p.data;
    })
  }


  Save(): void {
   
      this._updateUserService.AddUser(this.UpdateDto).subscribe(p => {
        if (p.isSuccess)
          this.router.navigate(['/users']);
        else
          for (let a of p.message.split('|'))
            this.Errors.push(a);
      }, (error) => {

        for (let a of error.error.message.split('|'))
          this.Errors.push(a);
      })
    
  }
}
