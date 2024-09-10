import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn:'root'
})
export class ResetPasswordService{
    url: string = environment.url+"api/users/ResetPassword";
    header:any;

    constructor(private http:HttpClient){}

    ResetPassword(userId:number){
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'bearer '+localStorage.getItem("token")
        });

        return this.http.post<any>(this.url+"?UserId="+userId,null,{headers:this.header}).pipe(
            map(p=>{
                return p;
            })
        );
    }
}