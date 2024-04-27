import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class DeleteQuestionService {
  url: string = environment.url + "api/Questions/DeleteQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  DeleteQuestion(questionId?: number): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.delete<any>(this.url + "?QuestionId=" + questionId, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    );
  }
}
