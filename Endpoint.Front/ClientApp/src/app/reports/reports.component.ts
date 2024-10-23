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
import { GetUserQueryRequestDto } from "../_models/report/GetUserQueryRequestDto";
import { GetUserQueryResponseDto } from "../_models/report/GetUserQueryResponseDto";
import { GetUserQueryService } from "../_services/report/getUserQueryService";
import { FormControl } from "@angular/forms";

@Component({
  templateUrl: './reports.component.html',
  selector: 'ReportsComponent'
})

export class ReportsComponent implements OnInit {


  access: string[] = [];
  refrenceUsers: KeyValueType[] = [];
  dateValue = new FormControl();
  dateValue2 = new FormControl();
  pageCount: number = 1;
  req: GetUserQueryRequestDto = {
    page: 1,
    size: 10,
    fromDate: "",
    toDate: ""
  };

  items?: GetUserQueryResponseDto[] = [];

  constructor(private _getUserQueryService: GetUserQueryService,
    private _getUsersByRolesService:GetUsersByRolesService) { }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

    this.getData();
  }

  search() {
    this.req.page = 1;
    this.getData();
  }

  getData() {
    this._getUserQueryService.GetReports(this.req!).subscribe(p => {
      this.items = p.data.items;
      this.pageCount = Math.floor((p.data.paging.totalItems % this.req.size!) == 0 ? (p.data.paging.totalItems / this.req.size!) : (p.data.paging.totalItems / this.req.size!) + 1)
      console.log(this.pageCount);
      console.log(this.req.page);
    })

    this._getUsersByRolesService.GetUsers([1, 2,3, 4,5,6]).subscribe(p => {
      this.refrenceUsers = p.data;
      console.log(this.refrenceUsers);
    })
  }

  inc() {
    this.req.page! += 1;
    this.getData();
  }
  dec() {
    this.req.page! -= 1;
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
