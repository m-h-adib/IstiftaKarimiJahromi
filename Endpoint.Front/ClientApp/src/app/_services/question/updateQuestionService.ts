import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UpdateQuestionDto } from "../../_models/question/updateQuestionDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class UpdateQuestionService {
  url: string = environment.url + "api/Questions/EditQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  UpdateQuestion(dto?: UpdateQuestionDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'multipart/form-data',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.put<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    );
  }
}
