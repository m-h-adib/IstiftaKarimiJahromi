import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { DataResultDto, ListDataResultDto } from "../../_models/common/ResultDto";
import { GetCountryDto } from "../../_models/country/GetCountryDto";
import { Observable } from "rxjs/internal/Observable";
import { map } from "rxjs/internal/operators/map";

@Injectable({
  providedIn: 'root'
})
export class GetOneCountryService {
  url: string = environment.url + "api/BaseInfos/GetOneCountry";
  header: any;

  constructor(private http: HttpClient) { }

  GetOneCountry(countryId: number): Observable<DataResultDto<GetCountryDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url + "?CountryId=" + countryId, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
