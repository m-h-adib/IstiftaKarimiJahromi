import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UpdateCountryDto } from "../../_models/country/UpdateCountryDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { UpdateJobCategoryDto } from "../../_models/jobCategory/UpdateJobCategoryDto";

@Injectable({
  providedIn:'root'
})
export class UpdateJobCategoryService {
  url: string = environment.url + "api/BaseInfos/UpdateJobCategory";
  header: any;

  constructor(private http: HttpClient) { }

  UpdateJobCategory(dto: UpdateJobCategoryDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.put<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
