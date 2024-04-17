import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class DeleteCountryService {
  url: string = environment.url + "api/BaseInfos/DeleteCountry";
  header: any;

  constructor(private http: HttpClient) { }

  DeleteCountry(countryId: number): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.delete<any>(this.url + "?CountryId=" + countryId, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
