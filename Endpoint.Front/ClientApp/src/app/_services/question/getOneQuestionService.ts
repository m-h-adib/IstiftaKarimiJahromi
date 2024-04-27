import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { DataResultDto } from "../../_models/common/ResultDto";
import { GetOneQuestionDto } from "../../_models/question/getOneQuestionDto";

@Injectable({
  providedIn:'root'
})
export class GetOneQuestionService {
  url?: string = environment.url + "api/Questions/GetOneQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  GetOneQuestion(questionId?: number): Observable<DataResultDto<GetOneQuestionDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url + "?QuestionId=" + questionId, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    );
  }
}
