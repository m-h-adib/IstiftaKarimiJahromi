import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class DeleteLanguageService {
  url: string = environment.url + "api/BaseInfos/DeleteLanguage";
  header: any;

  constructor(private http: HttpClient) { }

  DeleteLanguage(languageId: number): Observable<ResultDto> {
    this.header = new HttpHeaders({
      'content-type': 'application/json',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    return this.http.delete<any>(this.url + "?LanguageId=" + languageId, { headers: this.header }).pipe(
      map(p => {
        return p;
      })
    );
  }
}
