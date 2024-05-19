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

@Component({
  templateUrl: './questions.component.html',
  selector: 'QuestionsComponent'
})

export class QuestionsComponent implements OnInit {


  access: string[] = [];

  referenceToProcessorDto: ReferenceToProcessorDto = {};
  refrenceUsers: KeyValueType[] = [];
  deleteDialog?: boolean = false;
  refrenceDialog?: boolean = false;
  selectItem?: number;
  pageCount: number = 1;
  req: GetQuestionsRequestDto = {
    pageNumber: 1,
    pageSize: 10
  };

  items?:  GetQuestionsDto[] = [];

  constructor(private _getQuestionsService: GetQuestionsService,
    private _deleteQuestionService: DeleteQuestionService,
    private _getUsersByRolesService: GetUsersByRolesService,
    private _referenceToProcessorService:ReferenceToProcessorService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.getData();
  }

  getData() {
    this._getQuestionsService.GetQuestions(this.req!).subscribe(p => {
      this.items = p.data.items;
      this.pageCount = Math.floor((p.data.paging.totalItems % this.req.pageSize!) == 0 ? (p.data.paging.totalItems / this.req.pageSize!) : (p.data.paging.totalItems / this.req.pageSize!) + 1)
      console.log(this.pageCount);
      console.log(this.req.pageNumber);
    })

    this._getUsersByRolesService.GetUsers([1, 2, 4]).subscribe(p => {
      this.refrenceUsers = p.data;
      console.log(this.refrenceUsers);
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

  showDeleteDialog(id?: number) {
    this.selectItem = id;
    this.deleteDialog = !this.deleteDialog;
  }

  showRefrenceDialog(id?: number) {
    this.referenceToProcessorDto.questionId = id;
    this.refrenceDialog = !this.refrenceDialog;
  }

  deleteQuestion() {
    this._deleteQuestionService.DeleteQuestion(this.selectItem).subscribe(p => {
      const index = this.items?.findIndex(p => p.id == this.selectItem)!;
      this.items?.splice(index, 1);
      this.deleteDialog = false;
      this.selectItem = undefined;
    })
  }

  reference(){
    this._referenceToProcessorService.Refrenece(this.referenceToProcessorDto).subscribe(p => {
      alert(p.message);
      this.referenceToProcessorDto.questionId = 0;
      this.refrenceDialog = !this.refrenceDialog;
      this.getData();
    })
  }
}
