import { Component } from "@angular/core";
import { AddSessionDto } from "../_models/session/AddSessionDto";
import { AddSessionService } from "../_services/session/AddSessionService";
import { FormControl } from "@angular/forms";
import { GetUsersDto } from "../_models/user/GetUsersDto";
import { GetUsersService } from "../_services/user/getUsersService";

@Component({
    selector:'AddSessionComponent',
    templateUrl:'./addSession.component.html'
})
export class AddSessionComponent{
    AddDto:AddSessionDto={};
    dateValue = new FormControl();
    Users:GetUsersDto[]=[];
    Errors:string[]=[];

    constructor(private _addSessionService:AddSessionService,
        private _getUsersService:GetUsersService){}



        Save():void{

        }
}