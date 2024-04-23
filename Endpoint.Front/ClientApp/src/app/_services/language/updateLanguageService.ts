import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UpdateCountryDto } from "../../_models/country/UpdateCountryDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { UpdateJobCategoryDto } from "../../_models/jobCategory/UpdateJobCategoryDto";
import { UpdateLanguageDto } from "../../_models/language/UpdateLanguageDto";

@Injectable({
  providedIn:'root'
})
export class UpdateLanguageService {
  url: string = environment.url + "api/BaseInfos/UpdateLanguage";
  header: any;

  constructor(private http: HttpClient) { }

  UpdateLanguage(dto: UpdateLanguageDto): Observable<ResultDto> {
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
