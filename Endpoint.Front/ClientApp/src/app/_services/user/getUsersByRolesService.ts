import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetUsersRequestDto } from "../../_models/user/GetUsersRequestDto";
import { Observable, map } from "rxjs";
import { GetUsersDto } from "../../_models/user/GetUsersDto";
import { ListDataResultDto, PagingDataResultDto } from "../../_models/common/ResultDto";
import { KeyValueType } from "../../_models/common/keyValueType";

@Injectable({
  providedIn: 'root'
})
export class GetUsersByRolesService {
  url: string = environment.url + "api/BaseInfos/GetUsersByRoles";
  header: any;
  s: string = "";

  constructor(private http: HttpClient) { }

  GetUsers(dto?: number[]): Observable<ListDataResultDto<KeyValueType>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    this.s = "";
    for (var i = 0; i < dto!.length; i++) {
      this.s = this.s + "roles=" + dto![i] + "&";
    }

    return this.http.get<any>(this.url + "?roles=" + this.s, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
