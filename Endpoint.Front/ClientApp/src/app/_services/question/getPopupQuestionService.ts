import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { DataResultDto } from "src/app/_models/common/ResultDto";
import { GetPopupQuestionDto } from "src/app/_models/question/getPopupQuestionDto";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn:'root'
})
export class GetPopupQuestionService{
    url?:string=environment.url+"api/questions/GetPopupQuestion";
    header:any;

    constructor(private http:HttpClient){}

    GetPopup(questionId?:number):Observable<DataResultDto<GetPopupQuestionDto>>{
        this.header=new HttpHeaders({
            'content-type':'application/json',
            'authorization':'bearer '+localStorage.getItem("token")
        });

        return this.http.get<any>(this.url+"?QuestionId="+questionId,{headers:this.header}).pipe(
            map(p=>{
                return p;
            })
        );
    }
}