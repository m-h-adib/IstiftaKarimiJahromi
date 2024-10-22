import { Component, OnInit } from "@angular/core";
import { GetSessionDto } from "../_models/session/GetSessionDto";
import { GetSessionsService } from "../_services/session/GetSessionsService";
import { DeleteSessionService } from "../_services/session/DeleteSessionService";

@Component({
    templateUrl:'./sessions.component.html',
    selector:'SessionsComponent'
})
export class SessionsComponent implements OnInit{
  access: string[] = [];
  pageNumber?: number = 1;
    pageSize?:number=10;
    pageCount: number = 1;
  sessions?: GetSessionDto[] = [];
  sessionTitle?: string;
    
    deleteDialog?:boolean=false;
  selectSession?: number;


    constructor(private _getSessionsService:GetSessionsService,
      private _deleteSessionService:DeleteSessionService){}


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
        this.GetData();
    }

    getAccess(id: string): boolean {
      if (JSON.parse(localStorage.getItem("access")!).find((p: string) => p == id) != undefined)
        return true;
      else
        return false;
    }

  GetData(): void{
    this._getSessionsService.GetSessions(this.pageNumber, this.pageSize, this.sessionTitle).subscribe(p => {
            this.sessions=p.data.items;
            this.pageCount = Math.floor((p.data.paging.totalItems % this.pageSize!) == 0 ? (p.data.paging.totalItems / this.pageSize!) : (p.data.paging.totalItems / this.pageSize!) + 1)
        })
  }

  search() {
    this.pageNumber = 1;
    this.GetData();
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

      showDeleteDialog(id?: number){
        this.selectSession=id;
        this.deleteDialog=!this.deleteDialog;
      }

      deleteSession(){
        this._deleteSessionService.DeleteSession(this.selectSession!).subscribe(p=>{
          this.showDeleteDialog();
          this.GetData();
        })
      }



}
