import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AddUserDto } from "../_models/user/AddUserDto";
import { AddUserService } from "../_services/user/AddUserService";

@Component({
  templateUrl: './addAdmin.component.html',
  selector: 'AddAdminComponent'
})
export class AddAdminComponent implements OnInit {
  AddDto: AddUserDto = {
  };
  IsLoading: boolean = false;
  Errors: string[] = [];
  access: string[] = [];

  constructor(private _addUserService: AddUserService,
    private router: Router) { }


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "AddUser") == undefined)
      this.router.navigate(["/users"]);
  }


  Save(): void {
    if (this.AddDto.password == this.AddDto.repassword) {
      this._addUserService.AddUser(this.AddDto).subscribe(p => {
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
    else {
      this.Errors.push("رمز عبور با تکرار آن برابر نیست");
    }
  }
}
