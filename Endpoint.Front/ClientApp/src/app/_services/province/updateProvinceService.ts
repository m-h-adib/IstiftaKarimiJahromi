import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UpdateCountryDto } from "../../_models/country/UpdateCountryDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { UpdateProvinceDto } from "../../_models/province/UpdateProvinceDto";

@Injectable({
  providedIn:'root'
})
export class UpdateProvinceService {
  url: string = environment.url + "api/BaseInfos/UpdateProvince";
  header: any;

  constructor(private http: HttpClient) { }

  UpdateProvince(dto: UpdateProvinceDto): Observable<ResultDto> {
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
