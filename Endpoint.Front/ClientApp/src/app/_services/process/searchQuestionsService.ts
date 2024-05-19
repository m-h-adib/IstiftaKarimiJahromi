import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto, DataResultDto } from "../../_models/common/ResultDto";
import { GetQuestionsDto } from "../../_models/question/getQuestionsDto";
import { GetProcessQuestionsDto } from 'src/app/_models/process/getProcessQuestionsDto';
import { GetProcessQuestionsRequestDto } from 'src/app/_models/process/getProcessQuestionsRequestDto';
import { GetProcessCyclesDto } from 'src/app/_models/process/getProcessCyclesDto';
import { GetProcessCycleRequestDto } from 'src/app/_models/process/getProcessCycleRequestDto';
import { SearchQuestionRequestDto } from 'src/app/_models/process/searchQuestionRequestDto';
import { SearchQuestionDto } from 'src/app/_models/process/searechQuestionDto';

@Injectable({
  providedIn:'root'
})
export class SearchQuestionsService {
  url?: string = environment.url + "api/Processes/SearchQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  Search(dto: SearchQuestionRequestDto): Observable<DataResultDto<SearchQuestionDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url + "?title=" + dto.title+"&category="+dto.category, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    )
  }
}
