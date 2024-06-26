import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto, DataResultDto } from "../../_models/common/ResultDto";
import { GetResearchCycleRequestDto } from "../../_models/research/getResearchCycleRequestDto";
import { GetResearchCyclesDto } from "../../_models/research/getResearchCyclesDto";


@Injectable({
  providedIn:'root'
})
export class GetResearchCyclesService {
  url?: string = environment.url + "api/Researches/GetResearchCycles";
  header?: any;

  constructor(private http: HttpClient) { }

  GetCycles(dto: GetResearchCycleRequestDto): Observable<ListDataResultDto<GetResearchCyclesDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url + "?questionId=" + dto.questionId, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    )
  }
}
