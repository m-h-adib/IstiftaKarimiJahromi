import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto } from "../../_models/common/ResultDto";
import { GetResearchQuestionsRequestDto } from "../../_models/research/getResearchQuestionsRequestDto";
import { GetResearchQuestionsDto } from "../../_models/research/getResearchQuestionsDto";
import { GetEditorQuestionRequestDto } from 'src/app/_models/editor/getEditorQuestionRequestDto';
import { GetEditorQuestionDto } from 'src/app/_models/editor/getEditorQuestionDto';

@Injectable({
  providedIn:'root'
})
export class GetEditorQuestionsService {
  url?: string = environment.url + "api/Editors/GetEditorQuestions";
  header?: any;

  constructor(private http: HttpClient) { }

  GetQuestions(dto: GetEditorQuestionRequestDto): Observable<PagingDataResultDto<GetEditorQuestionDto>> {
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
