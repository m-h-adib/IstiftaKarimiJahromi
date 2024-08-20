import { Component, OnInit } from "@angular/core";
import { UpdateSessionDto } from "../_models/session/UpdateSessionDto";
import { GetSessionDto } from "../_models/session/GetSessionDto";
import { FormControl } from "@angular/forms";
import { GetUsersDto } from "../_models/user/GetUsersDto";
import { GetUsersService } from "../_services/user/getUsersService";
import { GetUsersRequestDto } from "../_models/user/GetUsersRequestDto";
import { GetOneSessionService } from "../_services/session/GetOneSessionService";
import { ActivatedRoute, Router } from "@angular/router";
import { GetOneSessionDto } from "../_models/session/GetOneSessionDto";
import { UpdateSessionService } from "../_services/session/UpdateSessionService";

@Component({
    selector:"UpdateSessionComponent",
    templateUrl:"./updateSession.component.html"
})
export class UpdateSessionComponent implements OnInit{
    dateValue = new FormControl();
    UpdateDto:UpdateSessionDto={};
    SessionDto:GetOneSessionDto={
        users:[],
        id:0
    };
    Users:GetUsersDto[]=[];
    GetUsersRequestDto:GetUsersRequestDto={
        pageNumber:1,
        pageSize:20,
        name:"",
        username:""
    }
    Errors:string[]=[];


    constructor(private route:ActivatedRoute,
        private router:Router,
        private _getUsersService:GetUsersService,
        private _getOneSessionService : GetOneSessionService, 
        private _updateSessionService:UpdateSessionService){}


    ngOnInit(): void {
        this._getUsersService.GetUsers(this.GetUsersRequestDto).subscribe(p=>{
            this.Users=p.data.items;
        })
        

        this.route.paramMap.subscribe( paramMap => {
            this._getOneSessionService.GetOneSession(Number.parseInt(paramMap.get('id')!.toString())).subscribe(p=>{
                this.SessionDto=p.data;
                this.UpdateDto.date=this.SessionDto.date;
                this.UpdateDto.id=this.SessionDto.id;
                this.UpdateDto.time=this.SessionDto.time;
                this.UpdateDto.users=this.SessionDto.users;
                this.UpdateDto.title=this.SessionDto.title;
            });
        })

       
       // this.SessionDto.users?.indexOf("1") !== -1
    }



    changeUser(id:number):void{
        if(this.UpdateDto.users?.filter(p=>p==id).length!>0)
            this.UpdateDto.users=this.UpdateDto.users?.filter(p=>p==id);
        else
        this.UpdateDto.users?.push(id);
    }

    Save():void{
        if(this.UpdateDto.title==undefined || this.UpdateDto.title=="")
            this.Errors.push("لطفا عنوان جلسه را وارد نمایید");
        if(this.UpdateDto.date==null || this.UpdateDto.date=="")
            this.Errors.push("لطفا تاریخ جلسه را انتخاب نمایید");
        if(this.UpdateDto.time==null || this.UpdateDto.time=="")
            this.Errors.push("لطفا ساعت جلسه را وارد نمایید");
        if(this.UpdateDto.users?.length==0)
            this.Errors.push("لطفا حاضرین جلسه را انتخاب نمایید");

        if(this.Errors.length==0){
            this._updateSessionService.UpdateSession(this.UpdateDto).subscribe(p=>{
                this.router.navigate(["sessions"]);
            })
        }
    }

}