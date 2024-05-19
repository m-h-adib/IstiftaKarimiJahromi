import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto, DataResultDto, ResultDto } from "../../_models/common/ResultDto";
import { GetQuestionsDto } from "../../_models/question/getQuestionsDto";
import { GetProcessQuestionsDto } from 'src/app/_models/process/getProcessQuestionsDto';
import { GetProcessQuestionsRequestDto } from 'src/app/_models/process/getProcessQuestionsRequestDto';
import { GetProcessCyclesDto } from 'src/app/_models/process/getProcessCyclesDto';
import { GetProcessCycleRequestDto } from 'src/app/_models/process/getProcessCycleRequestDto';
import { AddProcessQuestionDto } from 'src/app/_models/process/addProcessQuestionDto';

@Injectable({
  providedIn:'root'
})
export class AddProcessQuestionService {
  url: string = environment.url + "api/Processes/AddProcessQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  AddProcess(dto: AddProcessQuestionDto): Observable<ResultDto> {
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
