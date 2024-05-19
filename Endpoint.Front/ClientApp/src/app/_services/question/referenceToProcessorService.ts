import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto, ResultDto } from "../../_models/common/ResultDto";
import { GetQuestionsDto } from "../../_models/question/getQuestionsDto";
import { ReferenceToProcessorDto } from 'src/app/_models/question/referenceToProcessorDto';

@Injectable({
  providedIn:'root'
})
export class ReferenceToProcessorService {
  url: string = environment.url + "api/Questions/ReferenceToProcessor";
  header?: any;

  constructor(private http: HttpClient) { }

  Refrenece(dto: ReferenceToProcessorDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    dto.toUser=Number.parseInt(dto.toUser!.toString());

    return this.http.post<any>(this.url ,dto, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    )
  }
}
