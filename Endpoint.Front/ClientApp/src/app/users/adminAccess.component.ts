import { Component, OnInit } from "@angular/core";
import { GetUserAccessDto } from "../_models/user/GetUserAccessDto";
import { UpdateUserAccessDto } from "../_models/user/UpdateUserAccessDto";
import { GetUserAccessService } from "../_services/user/getUserAccessService";
import { ActivatedRoute, Router } from "@angular/router";
import { UpdateUserAccessService } from "../_services/user/UpdateUserAccessService";

@Component({
  templateUrl: './adminAccess.component.html',
  selector: 'AdminAccessComponent'
})
export class AdminAccessComponent implements OnInit {
  id?: string = undefined;
  access1: string[] = [];
  access: GetUserAccessDto[] = [];
  userAccess: number[] = [];
  updateDto: UpdateUserAccessDto = {};

  constructor(private _getUserAccessService: GetUserAccessService,
    private route: ActivatedRoute,
    private router: Router,
    private _updateUserAccessService: UpdateUserAccessService) { }


  ngOnInit(): void {
    this.access1 = JSON.parse(localStorage.getItem("access")!);
    if (this.access1.find(p => p == "ChangeUserAccess") == undefined)
      this.router.navigate(["/users"]);
    this.id = this.route.snapshot.paramMap.get("id")!;
    this.updateDto.userId = Number.parseInt(this.id);

    this._getUserAccessService.GetUserAccess(Number.parseInt(this.id)).subscribe(p => {
      this.access = p.data;
    })
  }

  Save(): void{
    this.userAccess = [];
    for (let a of this.access) {
      if (a.isAccess == true)
        this.userAccess.push(a.id!);

      for (let b of a.childs!) {
        if (b.isAccess == true)
          this.userAccess.push(b.id!);

        for (let c of b.childs!) {
          if (c.isAccess == true)
            this.userAccess.push(c.id!);
        }
      }
    }
    
    this.updateDto.access = this.userAccess;
    this._updateUserAccessService.UpdateUserAccess(this.updateDto).subscribe(p => {
      if (p.isSuccess)
        this.router.navigate(['/users']);
    })
  }
}
