import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto, DataResultDto } from "../../_models/common/ResultDto";
import { GetResearchCycleRequestDto } from "../../_models/research/getResearchCycleRequestDto";
import { GetResearchCyclesDto } from "../../_models/research/getResearchCyclesDto";
import { GetEditorCycleRequestDto } from 'src/app/_models/editor/getEditorCycleRequestDto';
import { GetEditorCycleDto } from 'src/app/_models/editor/getEditorCycleDto';


@Injectable({
  providedIn:'root'
})
export class GetEditorCyclesService {
  url?: string = environment.url + "api/Editors/GetEditorCycles";
  header?: any;

  constructor(private http: HttpClient) { }

  GetCycles(dto: GetEditorCycleRequestDto): Observable<ListDataResultDto<GetEditorCycleDto>> {
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
