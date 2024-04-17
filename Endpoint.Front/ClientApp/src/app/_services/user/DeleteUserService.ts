import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetUsersRequestDto } from "../../_models/user/GetUsersRequestDto";
import { Observable, map } from "rxjs";
import { GetUsersDto } from "../../_models/user/GetUsersDto";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn: 'root'
})
export class DeleteUserService {
  url: string = environment.url + "api/Users/DeleteUser";
  header: any;

  constructor(private http: HttpClient) { }

  DeleteUser(id: number): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.delete<any>(this.url + "?UserId=" + id, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
