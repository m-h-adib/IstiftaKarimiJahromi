import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { DataResultDto, ListDataResultDto, PagingDataResultDto } from "../../_models/common/ResultDto";
import { GetCountryDto } from "../../_models/country/GetCountryDto";
import { Observable } from "rxjs/internal/Observable";
import { map } from "rxjs/internal/operators/map";
import { GetArchiveQuestionDto } from 'src/app/_models/archive/getArchiveQuestionDto';
import { GetArchiveQuestionRequestDto } from 'src/app/_models/archive/getArchiveQuestionRequestDto';

@Injectable({
  providedIn: 'root'
})
export class GetArchiveQuestionService {
  url: string = environment.url + "api/Archive/GetArchiveQuestions";
  header: any;

  constructor(private http: HttpClient) { }

  ExportExcel(dto: GetArchiveQuestionRequestDto): Observable<Blob> {
    const headers = new HttpHeaders({
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    let params = new HttpParams();
    Object.entries(dto).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, value.toString());
      }
    });

    return this.http.get(this.url.replace('GetArchiveQuestions', 'ExportArchiveQuestionsExcel'), {
      headers,
      params,
      responseType: 'blob'
    });
  }

  GetArchives(dto?:GetArchiveQuestionRequestDto): Observable<PagingDataResultDto<GetArchiveQuestionDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url +
      "?PageNumber=" + dto?.PageNumber + 
      "&PageSize=" + dto?.PageSize+
      "&City="+dto?.City+
      "&CountryId="+ (dto?.CountryId!=undefined ? dto?.CountryId : null)+
      "&Education="+(dto?.Education!=undefined ? dto?.Education : null)+
      "&FirstName="+dto?.FirstName+
      "&FromAge="+dto?.FromAge+
      "&FromDate="+dto?.FromDate+
      "&Gender="+dto?.Gender+
      "&JobCategoryId="+dto?.JobCategoryId+
      "&LastName="+dto?.LastName+
      "&LastName="+dto?.LastName+
      "&MethodOfReceiveQuestionId="+dto?.MethodOfReceiveQuestionId+
      "&Mobile="+dto?.Mobile+
      "&ProvinceId="+dto?.ProvinceId+
      "&QuestionCategoryId="+dto?.QuestionCategoryId+
      "&Title=" + dto?.Title +
      "&Id=" + dto?.Id+
      "&ToAge="+dto?.ToAge+
      "&ToDate="+dto?.ToDate , { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
