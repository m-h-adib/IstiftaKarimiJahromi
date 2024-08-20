import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { ResultDto } from "src/app/_models/common/ResultDto";
import { AddSessionDto } from "src/app/_models/session/AddSessionDto";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn:'root'
})
export class AddSessionService{
    url:string=environment.url+"api/sessions/Addsession";
    header:any;

    constructor(private http:HttpClient){}

    AddSession(dto:AddSessionDto):Observable<ResultDto>{
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'bearer '+localStorage.getItem("token")
        });

        return this.http.post<any>(this.url,dto,{headers:this.header}).pipe(
            map(p=>{
                return p;
            })
        );
    }
}