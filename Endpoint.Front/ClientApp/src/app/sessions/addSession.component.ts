import { Component, OnInit } from "@angular/core";
import { AddSessionDto } from "../_models/session/AddSessionDto";
import { AddSessionService } from "../_services/session/AddSessionService";
import { FormControl } from "@angular/forms";
import { GetUsersDto } from "../_models/user/GetUsersDto";
import { GetUsersService } from "../_services/user/getUsersService";
import { GetUsersRequestDto } from '../_models/user/GetUsersRequestDto';
import { ActivatedRoute, Router } from "@angular/router";

@Component({
    selector:'AddSessionComponent',
    templateUrl:'./addSession.component.html'
})
export class AddSessionComponent implements OnInit{
    AddDto:AddSessionDto={
        users:[]
    };
    dateValue = new FormControl();
    Users:GetUsersDto[]=[];
    GetUsersRequestDto:GetUsersRequestDto={
        pageNumber:1,
        pageSize:1000,
        name:"",
        username:""
    };
    
    Errors:string[]=[];

    constructor(private router: Router,
        private route:ActivatedRoute,
        private _addSessionService:AddSessionService,
        private _getUsersService:GetUsersService){}
    
    
        ngOnInit(): void {
            this.route.paramMap.subscribe( paramMap => {
                this.AddDto.researchId = Number.parseInt(paramMap.get('researchid')!.toString());
            })

            this._getUsersService.GetUsers(this.GetUsersRequestDto).subscribe(p=>{
                this.Users=p.data.items;
            })
        }


        


        Save():void{
            if(this.AddDto.title==undefined || this.AddDto.title=="")
                this.Errors.push("لطفا عنوان جلسه را وارد نمایید");
            if(this.AddDto.date==null || this.AddDto.date=="")
                this.Errors.push("لطفا تاریخ جلسه را انتخاب نمایید");
            if(this.AddDto.time==null || this.AddDto.time=="")
                this.Errors.push("لطفا ساعت جلسه را وارد نمایید");
            if(this.AddDto.users?.length==0)
                this.Errors.push("لطفا حاضرین جلسه را انتخاب نمایید");

            if(this.Errors.length==0){
                this._addSessionService.AddSession(this.AddDto).subscribe(p=>{
                    this.router.navigate(["sessions"]);
                })
            }
        }

        changeUser(id:number):void{
            if(this.AddDto.users?.filter(p=>p==id).length!>0)
                this.AddDto.users=this.AddDto.users?.filter(p=>p==id);
            else
            this.AddDto.users?.push(id);
        }

    }