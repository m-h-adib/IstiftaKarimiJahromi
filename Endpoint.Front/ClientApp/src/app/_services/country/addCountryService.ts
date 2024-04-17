import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { AddCountryDto } from "../../_models/country/AddCountryDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn:'root'
})
export class AddCountryService {
  url: string = environment.url + "api/BaseInfos/AddCountry";
  header: any;

  constructor(private http: HttpClient) { }

  AddCountry(dto: AddCountryDto): Observable<ResultDto> {
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
