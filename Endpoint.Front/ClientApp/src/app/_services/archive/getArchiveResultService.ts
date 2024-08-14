import { Injectable } from '@angular/core';
import { GetArchiveStepDto } from 'src/app/_models/archive/getArchiveStepDto';
import { ListDataResultDto } from 'src/app/_models/common/ResultDto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from "rxjs/internal/Observable";
import { GetArchiveResultDto } from 'src/app/_models/archive/getArchiveResultDto';
import { map } from "rxjs/internal/operators/map";

@Injectable({
    providedIn:"root"
})

export class GetArchiveResultService{
    url: string = environment.url + "api/Archive/GetQuestionResults";
  header: any;

  constructor(private http: HttpClient) { }

  GetResults(QuestionId?:number): Observable<ListDataResultDto<GetArchiveResultDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url + "?QuestionId=" + QuestionId  , { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}