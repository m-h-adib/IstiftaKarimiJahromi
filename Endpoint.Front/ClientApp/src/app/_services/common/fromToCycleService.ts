import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.prod";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetQuestionsRequestDto } from "../../_models/question/getQuestionsRequestDto";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto, DataResultDto, ResultDto } from "../../_models/common/ResultDto";
import { FromToCycleDto } from "../../_models/common/fromToCycleDto";

@Injectable({
  providedIn:'root'
})
export class FromToCycleService {
  url: string = environment.url + "api/baseinfos/FromToCycle";
  header?: any;

  constructor(private http: HttpClient) { }

  FromToCycle(dto: FromToCycleDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.post<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    )
  }
}
