import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto } from "../../_models/common/ResultDto";
import { GetQuestionsDto } from "../../_models/question/getQuestionsDto";

@Injectable({
  providedIn:'root'
})
export class GetQuestionsService {
  url?: string = environment.url + "api/Questions/GetQuestions";
  header?: any;

  constructor(private http: HttpClient) { }

  GetQuestions(dto: GetQuestionsRequestDto): Observable<PagingDataResultDto<GetQuestionsDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url + "?PageNumber=" + dto.pageNumber + "&PageSize=" + dto.pageSize + "&Title=" + (dto.title != undefined ? dto.title : ""), { headers: this.header }).pipe(
      map(p => {
        return p
      })
    )
  }
}
