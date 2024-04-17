import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetUsersRequestDto } from "../../_models/user/GetUsersRequestDto";
import { Observable, map } from "rxjs";
import { GetUsersDto } from "../../_models/user/GetUsersDto";
import { AddUserDto } from "../../_models/user/AddUserDto";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn: 'root'
})
export class AddUserService {
  url: string = environment.url + "api/Users/AddUser";
  header: any;

  constructor(private http: HttpClient) { }

  AddUser(dto: AddUserDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.post<any>(this.url, dto, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
