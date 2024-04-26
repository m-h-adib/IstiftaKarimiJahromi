import { OnInit } from "@angular/core";

export class StoreService {
  public loginStatus: boolean = false; // 0 = not logged in, 1 = logged in
  public isAdmin: boolean = false;
  public QuestionCategorySelectItem?: number = undefined;

  setLoginStatus(status: boolean) {
    this.loginStatus = status;
  }
  setIsAdmin(status: boolean) {
    this.isAdmin = status;
  }

  SetQusetionCategory(id?: number) {
    this.QuestionCategorySelectItem = id;
  }
}
