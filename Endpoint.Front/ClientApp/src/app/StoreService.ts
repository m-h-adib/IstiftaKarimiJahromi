import { OnInit } from "@angular/core";

export class StoreService {
  public loginStatus: boolean = false; // 0 = not logged in, 1 = logged in
  public isAdmin: boolean = false;

  setLoginStatus(status: boolean) {
    this.loginStatus = status;
  }
  setIsAdmin(status: boolean) {
    this.isAdmin = status;
  }
}
