import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { LoginRequestModel } from "../../_models/login/loginRequestModel";
import { LoginResponseModel } from "../../_models/login/loginResponseModel";
import { environment } from "../../../environments/environment";
import { DataResultDto } from "../../_models/common/ResultDto";


@Injectable({
  providedIn:'root'
})
export class LoginService {
  url: string = environment.url + "api/Identity/Login";
  header: any;

  constructor(private http: HttpClient) { }

  login(dto: LoginRequestModel): Observable<DataResultDto<LoginResponseModel>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json'
    });

    return this.http.post<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
