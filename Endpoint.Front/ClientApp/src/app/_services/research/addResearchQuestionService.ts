import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto, DataResultDto, ResultDto } from "../../_models/common/ResultDto";
import { AddResearchQuestionDto } from "../../_models/research/addResearchQuestionDto";

@Injectable({
  providedIn:'root'
})
export class AddResearchQuestionService {
  url: string = environment.url + "api/Researches/AddResearchQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  AddProcess(dto: AddResearchQuestionDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.post<any>(this.url,dto, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    )
  }
}
