import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AddProvinceDto } from "../../_models/province/AddProvinceDto";

@Injectable({
  providedIn:'root'
})
export class AddProvinceService {
  url: string = environment.url + "api/BaseInfos/AddProvince";
  header: any;

  constructor(private http: HttpClient) { }

  AddProvince(dto: AddProvinceDto): Observable<ResultDto> {
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
