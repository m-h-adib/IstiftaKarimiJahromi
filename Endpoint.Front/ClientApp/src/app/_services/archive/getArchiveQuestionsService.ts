import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { DataResultDto, ListDataResultDto } from "../../_models/common/ResultDto";
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

  GetArchives(dto?:GetArchiveQuestionRequestDto): Observable<ListDataResultDto<GetArchiveQuestionDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url +
      "?PageNumber=" + dto?.PageNumber + 
      "&PageSize=" + dto?.PageSize+
      "&City="+dto?.City+
      "&CountryId="+dto?.CountryId+
      "&Education="+dto?.Education+
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
      "&Title="+dto?.Title+
      "&ToAge="+dto?.ToAge+
      "&ToDate="+dto?.ToDate , { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
