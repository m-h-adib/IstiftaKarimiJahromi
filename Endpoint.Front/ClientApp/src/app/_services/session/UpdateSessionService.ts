import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { ResultDto } from "src/app/_models/common/ResultDto";
import { AddSessionDto } from "src/app/_models/session/AddSessionDto";
import { UpdateSessionDto } from "src/app/_models/session/UpdateSessionDto";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn:'root'
})
export class UpdateSessionService{
    url:string=environment.url+"api/sesstions/UpdateSession";
    header:any;

    constructor(private http:HttpClient){}

    AddSession(dto:UpdateSessionDto):Observable<ResultDto>{
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'bearer '+localStorage.getItem("token")
        });

        return this.http.put<any>(this.url,dto,{headers:this.header}).pipe(
            map(p=>{
                return p;
            })
        );
    }
}