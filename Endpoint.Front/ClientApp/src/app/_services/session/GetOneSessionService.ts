import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { DataResultDto } from 'src/app/_models/common/ResultDto';
import { GetSessionDto } from 'src/app/_models/session/GetSessionDto';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn:"root"
})
export class GetOneSessionService{
    url:string=environment.url+"api/sessions/GetOneSession";
    header:any;

    constructor(private http:HttpClient){}

    GetOneSession(id:number):Observable<DataResultDto<GetSessionDto>>{
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'bearer '+localStorage.getItem("token")
        });

        return this.http.get<any>(this.url+"?Id="+id,{headers:this.header}).pipe(
            map(p=>{
                return p;
            })
        );
    }
}