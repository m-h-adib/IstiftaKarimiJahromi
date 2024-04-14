import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { StoreService } from "../StoreService";
import { Utils } from "../_utils/Utils";

@Component({
  selector:'app-login',
  templateUrl:'./login.component.html'
})
export class LoginComponent {
  username: string = "";
  password: string = "";
  Loading: boolean = false;
  Error: string = "";

  constructor(
    private _store: StoreService,
    private router: Router) { }

  Login(): void {
    this.username = Utils.transformNonNumber(this.username);
    this.password = Utils.transformNonNumber(this.password);
    //this._loginService.Login(this.username, this.password).subscribe(p => {
    //  if (p.isSuccess) {
    //    localStorage.setItem("token", p.data.token!);
    //    localStorage.setItem("name", p.data.name!);
    //    localStorage.setItem("status", p.data.status!);
    //    this._store.setLoginStatus(true);
    //    this.router.navigate(["/"]);
    //  }
    //  else {
    //    this.Error = p.message
    //  }
    //},
    //  (error) => {
    //    this.Error = error.error.message;
    //  })
  }
}
