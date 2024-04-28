import { Component, OnInit } from "@angular/core";
import { GetQuestionsService } from "../_services/question/getQuestionsService";
import { GetQuestionsRequestDto } from "../_models/question/getQuestionsRequestDto";
import { GetQuestionsDto } from "../_models/question/getQuestionsDto";
import { Utils } from "../_utils/Utils";

@Component({
  templateUrl: './questions.component.html',
  selector: 'QuestionsComponent'
})

export class QuestionsComponent implements OnInit {

  access: string[] = [];
  
  pageCount: number = 1;
  req: GetQuestionsRequestDto = {
    pageNumber: 1,
    pageSize: 10
  };

  items?:  GetQuestionsDto[] = [];

  constructor(private _getQuestionsService: GetQuestionsService) { }

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
}
