import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { AddCountryDto } from "../../_models/country/AddCountryDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AddJobCategoryDto } from "../../_models/jobCategory/AddJobCategoryDto";

@Injectable({
  providedIn:'root'
})
export class AddJobCategoryService {
  url: string = environment.url + "api/BaseInfos/AddJobCategory";
  header: any;

  constructor(private http: HttpClient) { }

  AddJobCategory(dto: AddJobCategoryDto): Observable<ResultDto> {
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
