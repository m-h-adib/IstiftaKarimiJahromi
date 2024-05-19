import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto } from "../../_models/common/ResultDto";
import { GetQuestionsDto } from "../../_models/question/getQuestionsDto";
import { GetProcessQuestionsDto } from 'src/app/_models/process/getProcessQuestionsDto';
import { GetProcessQuestionsRequestDto } from 'src/app/_models/process/getProcessQuestionsRequestDto';

@Injectable({
  providedIn:'root'
})
export class GetProcessQuestionsService {
  url?: string = environment.url + "api/Processes/GetQuestions";
  header?: any;

  constructor(private http: HttpClient) { }

  GetQuestions(dto: GetProcessQuestionsRequestDto): Observable<PagingDataResultDto<GetProcessQuestionsDto>> {
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
