import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from "rxjs";
import { PagingDataResultDto } from 'src/app/_models/common/ResultDto';
import { GetSessionDto } from 'src/app/_models/session/GetSessionDto';

@Injectable({
    providedIn:'root'
})
export class GetSessionsService{
    url:string=environment.url+"api/Sessions/GetSessions";
    header:any;

    constructor(private http:HttpClient){}

    GetSessions(pageNumber?:number,pageSize?:number):Observable<PagingDataResultDto<GetSessionDto>>{
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'baerer '+localStorage.getItem("token")
        });

        return this.http.get<any>(this.url+"?PageNumber="+pageNumber+"&PageSize="+pageSize,{headers:this.header}).pipe(
            map(p => {
              return p;
            })
          )
    }
}