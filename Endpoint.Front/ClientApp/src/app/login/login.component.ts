import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { StoreService } from "../StoreService";
import { Utils } from "../_utils/Utils";
import { LoginService } from "../_services/identity/loginService";
import { LoginRequestModel } from "../_models/login/loginRequestModel";

@Component({
  selector:'app-login',
  templateUrl:'./login.component.html'
})
export class LoginComponent {
  loginDto: LoginRequestModel = {};
  Loading: boolean = false;
  Error: string = "";

  constructor(
    private _loginService: LoginService,
    private _store: StoreService,
    private router: Router) { }

  Login(): void {
    console.log(this.loginDto);
    this.loginDto.username = Utils.transformNonNumber(this.loginDto.username!);
    this.loginDto.password = Utils.transformNonNumber(this.loginDto.password!);
    this._loginService.login(this.loginDto).subscribe(p => {
      console.log(p);
      if (p.isSuccess) {
        localStorage.setItem("token", p.data.token!);
        localStorage.setItem("name", p.data.name!);
        localStorage.setItem("status", p.data.status.toString()!);
        this._store.setLoginStatus(true);
        this.router.navigate(["/"]);
      }
      else {
        //this.Error = p.message
      }
    },
      (error) => {
        console.log(error);
        //this.Error = error.error.message;
      })
  }
}
