import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetUsersRequestDto } from "../../_models/user/GetUsersRequestDto";
import { Observable, map } from "rxjs";
import { GetOneUserDto } from "../../_models/user/GetOneUserDto";
import { DataResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn: 'root'
})
export class GetOneUserService {
  url: string = environment.url + "api/Users/GetOneUser";
  header: any;

  constructor(private http: HttpClient) { }

  GetUsers(userId: number): Observable<DataResultDto<GetOneUserDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url + "?UserId=" + userId , { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
