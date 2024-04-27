import { Component, OnInit } from "@angular/core";
import { GetQuestionsService } from "../_services/question/getQuestionsService";
import { GetQuestionsRequestDto } from "../_models/question/getQuestionsRequestDto";
import { GetQuestionsDto } from "../_models/question/getQuestionsDto";

@Component({
  templateUrl: './questions.component.html',
  selector: 'QuestionsComponent'
})

export class QuestionsComponent implements OnInit {

  access: string[] = [];
  pageCount: number = 1;
  req: GetQuestionsRequestDto = {};

  items?:  GetQuestionsDto[] = [];

  constructor(private _getQuestionsService: GetQuestionsService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.getData();
  }

  getData() {
    this._getQuestionsService.GetQuestions(this.req!).subscribe(p => {
      this.items = p.data.items;
      this.pageCount = ((p.data.paging.totalItems % this.req.pageSize!) == 0 ? (p.data.paging.totalItems / this.req.pageSize!) : (p.data.paging.totalItems / this.req.pageSize!)+1)
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
}
