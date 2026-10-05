import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { environment } from "../../../environments/environment.prod";

@Injectable({
  providedIn: "root"
})
export class GetQuestionPrintService {
  url = environment.url + "api/Archive/GetQuestionPrint";

  constructor(private http: HttpClient) {}

  GetPrint(questionId: number): Observable<any> {
    const headers = new HttpHeaders({
      "content-type": "application/json",
      "authorization": "bearer " + localStorage.getItem("token")
    });

    return this.http.get<any>(this.url + "?QuestionId=" + questionId, { headers }).pipe(
      map(p => p)
    );
  }

  GetPrintPdf(questionId: number, format: "A5" | "A4"): Observable<Blob> {
    const headers = new HttpHeaders({
      "authorization": "bearer " + localStorage.getItem("token")
    });

    return this.http.get(
      environment.url +
        "api/Archive/GetQuestionPrintPdf?QuestionId=" +
        questionId +
        "&format=" +
        format,
      {
        headers,
        responseType: "blob"
      }
    );
  }
}
