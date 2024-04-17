import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { environment } from "../../../environments/environment";
import { ListDataResultDto } from "../../_models/common/ResultDto";
import { GetMenuAccessDto } from "../../_models/login/getMenuAccessDto";


@Injectable({
  providedIn:'root'
})
export class GetMenuAccessService {
  url: string = environment.url + "api/Identity/GetMunuAccess";
  header: any;

  constructor(private http: HttpClient) { }

  GetMenuAccess(): Observable<ListDataResultDto<GetMenuAccessDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
