import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class DeleteArchiveQuestionService {
  url: string = environment.url + "api/Archive/DeleteArchiveQuestion";
  header: any;

  constructor(private http: HttpClient) { }

  Delete(id: number): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.delete<any>(this.url + "?QuestionId=" + id, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
