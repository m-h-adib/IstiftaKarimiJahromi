import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { AddCountryDto } from "../../_models/country/AddCountryDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AddJobCategoryDto } from "../../_models/jobCategory/AddJobCategoryDto";
import { AddLanguageDto } from "../../_models/language/AddLanguageDto";
import { AddMethodOfRecieveQuestionDto } from "../../_models/methodRecieveQuestion/AddMethodOfRecieveQuestionDto";

@Injectable({
  providedIn:'root'
})
export class AddMethodOfRecieveQuestionService {
  url: string = environment.url + "api/BaseInfos/AddMethodOfReceiveQuestion";
  header: any;

  constructor(private http: HttpClient) { }

  AddMethodOfRecieveQuestion(dto: AddMethodOfRecieveQuestionDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.post<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
