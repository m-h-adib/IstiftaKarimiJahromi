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

  GetSessions(pageNumber?: number, pageSize?: number, title?: string): Observable<PagingDataResultDto<GetSessionDto>>{
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'bearer '+localStorage.getItem("token")
        });

    return this.http.get<any>(this.url + "?PageNumber=" + pageNumber + "&PageSize=" + pageSize + "&title=" + title, { headers: this.header }).pipe(
            map(p => {
              return p;
            })
          )
    }
}
