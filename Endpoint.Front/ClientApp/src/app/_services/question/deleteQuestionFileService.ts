import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class DeleteQuestionFileService {
  url: string = environment.url + "api/Questions/DeleteQuestionFile";
  header?: any;

  constructor(private http: HttpClient) { }

  DeleteQuestionFile(questionFileId?: number): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.delete<any>(this.url + "?QuestionFileId=" + questionFileId, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    );
  }
}
