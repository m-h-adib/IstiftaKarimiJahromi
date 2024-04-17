import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { GetUsersRequestDto } from "../../_models/user/GetUsersRequestDto";
import { Observable, map } from "rxjs";
import { GetUsersDto } from "../../_models/user/GetUsersDto";
import { PagingDataResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn: 'root'
})
export class GetUsersService {
  url: string = environment.url + "api/Users/GetUsers";
  header: any;

  constructor(private http: HttpClient) { }

  GetUsers(dto: GetUsersRequestDto): Observable<PagingDataResultDto<GetUsersDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem('token')
    });

    return this.http.get<any>(this.url + "?PageNember=" + dto.pageNumber + "&PageSize=" + dto.pageSize + "&Name=" + dto.name + "&Username=" + dto.username, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    )
  }
}
