import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Observable, map } from "rxjs";
import { ListDataResultDto } from "../../_models/common/ResultDto";
import { GetQuestionCategoryDto } from "../../_models/questionCategory/GetQuestionCategoryDto";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn:'root'
})

export class GetQuestionCategoriesService {
  url: string = environment.url + "api/BaseInfos/GetQuestionCategories";
  header: any;

  constructor(private http: HttpClient) { }

  GetQuestionCategories(): Observable<ListDataResultDto<GetQuestionCategoryDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
