import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Observable, map } from "rxjs";
import { ListDataResultDto, PagingDataResultDto } from "../../_models/common/ResultDto";
import { GetUserQueryRequestDto } from "../../_models/report/GetUserQueryRequestDto";
import { GetUserQueryResponseDto } from "../../_models/report/GetUserQueryResponseDto";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})

export class GetUserQueryService {
  url: string = environment.url + "api/Reports";
  header: any;

  constructor(private http: HttpClient) { }

  GetReports(dto: GetUserQueryRequestDto): Observable<PagingDataResultDto<GetUserQueryResponseDto>> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url + "?userId=" + dto.userId + "&fromDate=" + dto.fromDate + "&toDate=" + dto.toDate + "&type=" + dto.type + "&page=" + dto.page + "&size=" + dto.size, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
