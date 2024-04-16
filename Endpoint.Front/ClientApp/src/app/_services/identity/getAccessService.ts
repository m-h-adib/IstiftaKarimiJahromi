import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { environment } from "../../../environments/environment";

@Injectable({
  providedIn:'root'
})
export class GetAccessService {
  url: string = environment.url + "api/Identity/GetAccess";
  header: any;

  constructor(private http: HttpClient) { }

  GetMenuAccess(): Observable<any> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url + "?role=" + localStorage.getItem("status"), { headers: this.header }).pipe(
      map(p => {

        return p;
      })
    )
  }
}
