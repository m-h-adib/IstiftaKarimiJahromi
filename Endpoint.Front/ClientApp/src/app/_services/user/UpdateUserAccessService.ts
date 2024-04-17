import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetUsersRequestDto } from "../../_models/user/GetUsersRequestDto";
import { Observable, map } from "rxjs";
import { GetUsersDto } from "../../_models/user/GetUsersDto";
import { AddUserDto } from "../../_models/user/AddUserDto";
import { UpdateUserAccessDto } from "../../_models/user/UpdateUserAccessDto";
import { ResultDto } from "../../_models/common/ResultDto";


@Injectable({
  providedIn: 'root'
})
export class UpdateUserAccessService {
  url: string = environment.url + "api/Users/UpdateUserAccess";
  header: any;

  constructor(private http: HttpClient) { }

  UpdateUserAccess(dto: UpdateUserAccessDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.put<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
