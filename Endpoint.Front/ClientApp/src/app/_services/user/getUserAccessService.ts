import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetUsersRequestDto } from "../../_models/user/GetUsersRequestDto";
import { Observable, map } from "rxjs";
import { GetUsersDto } from "../../_models/user/GetUsersDto";
import { GetUserAccessDto } from "../../_models/user/GetUserAccessDto";
import { ListDataResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn: 'root'
})
export class GetUserAccessService {
  url: string = environment.url + "api/Users/GetUserAccess";
  header: any;

  constructor(private http: HttpClient) { }

  GetUserAccess(userId: number): Observable<ListDataResultDto<GetUserAccessDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url + "?UserId=" + userId, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
