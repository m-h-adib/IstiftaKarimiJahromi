import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AddQuestionDto } from "../../_models/question/addQuestionDto";
import { Observable, map } from "rxjs";
import { ResultDto } from "../../_models/common/ResultDto";

@Injectable({
  providedIn:'root'
})
export class AddQuestionService {
  url: string = environment.url + "api/Questions/AddQuestion";
  header?: any;

  constructor(private http: HttpClient) { }

  AddQuestion(dto: AddQuestionDto): Observable<ResultDto> {
    this.header = new HttpHeaders({
     // 'content-type': 'multipart/form-data',
      'authorization': 'bearer ' + localStorage.getItem("token")
    });

    const body = new FormData();

    body.append("title", dto?.title!);
    body.append("address", dto?.address!);
    body.append("questionDate", dto?.questionDate!);
    body.append("mobile", dto!.mobile!);
    body.append("provinceId", (dto.provinceId != undefined ? dto!.provinceId!.toString() : ''));
    body.append("age", (dto.age != undefined ? dto!.age!.toString() : ''));
    body.append("city", dto!.city!);
    body.append("countryId", (dto.countryId != undefined ? dto!.countryId!.toString() : ''));
    body.append("education", (dto.education != undefined ? dto!.education!.toString() : ''));
    body.append("email", dto!.email!);
    body.append("firstName", dto!.firstName!);
    body.append("gender", (dto.gender != undefined ? dto!.gender!.toString() : ''));
    body.append("job", dto!.job!);
    body.append("jobCategoryId", (dto.jobCategoryId != undefined ? dto!.jobCategoryId!.toString() : ''));
    body.append("lastName", dto!.lastName!);
    body.append("methodOfReceiveQuestionId", (dto.methodOfReceiveQuestionId != undefined ? dto!.methodOfReceiveQuestionId!.toString() : ''));
    body.append("questionCategoryId", (dto.questionCategoryId != undefined ?  dto!.questionCategoryId!.toString() : ''));
    body.append("questionText", dto!.questionText!);

    console.log(dto);
    dto.attachs?.forEach((obj) => {
      body.append("attachs", obj);
    })
    

    

    return this.http.post<any>(this.url, body, { headers: this.header }).pipe(
      map(p => {
        return p
      })
    );
  }
}
