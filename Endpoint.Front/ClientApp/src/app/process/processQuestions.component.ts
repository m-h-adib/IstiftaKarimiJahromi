import { Component, OnInit } from "@angular/core";
import { GetQuestionsService } from "../_services/question/getQuestionsService";
import { GetQuestionsRequestDto } from "../_models/question/getQuestionsRequestDto";
import { GetQuestionsDto } from "../_models/question/getQuestionsDto";
import { Utils } from "../_utils/Utils";
import { DeleteQuestionService } from "../_services/question/deleteQuestionService";
import { ReferenceToProcessorDto } from "../_models/question/referenceToProcessorDto";
import { GetUsersByRolesService } from "../_services/user/getUsersByRolesService";
import { KeyValueType } from "../_models/common/keyValueType";
import { ReferenceToProcessorService } from '../_services/question/referenceToProcessorService';
import { GetProcessQuestionsRequestDto } from '../_models/process/getProcessQuestionsRequestDto';
import { GetProcessQuestionsService } from '../_services/process/getProcessQuestionsService';
import { GetProcessQuestionsDto } from '../_models/process/getProcessQuestionsDto';
import { QuestionPopupComponent } from "../questions/questionPopup.component";

@Component({
  templateUrl: './processQuestions.component.html',
  selector: 'ProcessQuestionsComponent'
})

export class ProcessQuestionsComponent implements OnInit {


  access: string[] = [];

  selectItem?: number;
  pageCount: number = 1;
  req: GetProcessQuestionsRequestDto = {
    pageNumber: 1,
    pageSize: 10
  };

  items?:  GetProcessQuestionsDto[] = [];

  constructor(private _getProcessQuestionsService: GetProcessQuestionsService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.getData();
  }

  getData() {
    this._getProcessQuestionsService.GetQuestions(this.req!).subscribe(p => {
      this.items = p.data.items;
      this.pageCount = Math.floor((p.data.paging.totalItems % this.req.pageSize!) == 0 ? (p.data.paging.totalItems / this.req.pageSize!) : (p.data.paging.totalItems / this.req.pageSize!) + 1)
    })
  }

  inc() {
    this.req.pageNumber! += 1;
    this.getData();
  }
  dec() {
    this.req.pageNumber! -= 1;
    this.getData();
  }

  change() {
    this.getData();
  }


  getAccess(id: string): boolean {

    if (JSON.parse(localStorage.getItem("access")!).find((p: string) => p == id) != undefined)
      return true;
    else
      return false;
  }

 

  /*showRefrenceDialog(id?: number) {
    this.referenceToProcessorDto.questionId = id;
    this.refrenceDialog = !this.refrenceDialog;
  }


  reference(){
    this._referenceToProcessorService.Refrenece(this.referenceToProcessorDto).subscribe(p => {
      alert(p.message);
      this.referenceToProcessorDto.questionId = 0;
      this.refrenceDialog = !this.refrenceDialog;
      this.getData();
    })
  }*/
}
