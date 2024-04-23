import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { DataResultDto, ListDataResultDto } from "../../_models/common/ResultDto";
import { GetCountryDto } from "../../_models/country/GetCountryDto";
import { Observable } from "rxjs/internal/Observable";
import { map } from "rxjs/internal/operators/map";
import { GetJobCategoryDto } from "../../_models/jobCategory/GetJobCategoryDto";

@Injectable({
  providedIn: 'root'
})
export class GetOneJobCategoryService {
  url: string = environment.url + "api/BaseInfos/GetOneJobCategory";
  header: any;

  constructor(private http: HttpClient) { }

  GetOneJobCategory(jobCategoryId: number): Observable<DataResultDto<GetJobCategoryDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url + "?JobCategoryId=" + jobCategoryId, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
