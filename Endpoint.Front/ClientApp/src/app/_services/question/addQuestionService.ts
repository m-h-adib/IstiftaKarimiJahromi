import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AddQuestionDto } from "../../_models/question/addQuestionDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class AddQuestionService {
  url: string = environment.url + "api/Questions/AddQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  AddQuestion(dto?: AddQuestionDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
     // 'content-type': 'multipart/form-data',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    let body = new FormData();
    body.append("title", dto?.title!);
    body.append("address", dto?.address!);
    body.append("questionDate", dto?.questionDate!);
   


    return this.http.post<any>(this.url, body, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    );
  }
}
