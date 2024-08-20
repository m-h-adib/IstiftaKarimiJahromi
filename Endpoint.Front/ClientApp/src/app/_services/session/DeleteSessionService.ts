import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { ResultDto } from "src/app/_models/common/ResultDto";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn:'root'
})

export class DeleteSession{
    url:string=environment.url+"api/sessions/DeleteSession";
    header:any;

    constructor(private http:HttpClient){}

    DeleteSession(id:number):Observable<ResultDto>{
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'bearer '+localStorage.getItem("token")
        });

        return this.http.delete<any>(this.url+"?Id="+id,{headers:this.header}).pipe(
            map(p=>{
                return p;
            })
        );
    }
}