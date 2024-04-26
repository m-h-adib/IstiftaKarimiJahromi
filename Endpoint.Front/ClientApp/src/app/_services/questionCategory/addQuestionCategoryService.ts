import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { AddQuestionCategoryDto } from "../../_models/questionCategory/AddQuestionCategoryDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn:'root'
})
export class AddQuestionCategoryService {
  url: string = environment.url + "api/BaseInfos/AddQuestionCategory";
  header: any;

  constructor(private http: HttpClient) { }

  AddQuestionCategory(dto: AddQuestionCategoryDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.post<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
