import { Component, OnInit } from "@angular/core";
import { GetSessionDto } from "../_models/session/GetSessionDto";
import { GetSessionsService } from "../_services/session/GetSessionsService";

@Component({
    templateUrl:'./sessions.component.html',
    selector:'SessionsComponent'
})
export class SessionsComponent implements OnInit{
    pageNumber?:number=1;
    pageSize?:number=10;
    pageCount: number = 1;
    sessions?:GetSessionDto[]=[];

    constructor(private _getSessionsService:GetSessionsService){}


    ngOnInit(): void {
        this.GetData();
    }


    GetData():void{
        this._getSessionsService.GetSessions(this.pageNumber,this.pageSize).subscribe(p=>{
            this.sessions=p.data.items;
            this.pageCount = Math.floor((p.data.paging.totalItems % this.pageSize!) == 0 ? (p.data.paging.totalItems / this.pageSize!) : (p.data.paging.totalItems / this.pageSize!) + 1)
        })
    }

    inc() {
        this.pageNumber! += 1;
        this.GetData();
      }
      dec() {
        this.pageNumber! -= 1;
        this.GetData();
      }
    
      change() {
        this.GetData();
      }
}