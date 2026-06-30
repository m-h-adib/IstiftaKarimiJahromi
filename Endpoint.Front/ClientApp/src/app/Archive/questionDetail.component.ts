import { Component, Input, OnInit } from '@angular/core';
import { AddQuestionDto } from '../_models/question/addQuestionDto';
import { GetArchiveQuestionDto } from '../_models/archive/getArchiveQuestionDto';
import { SendAnswerDateDto } from '../_models/archive/sendAnswerDateDto';
import { FormControl } from '@angular/forms';
import { SendAnswerDateService } from '../_services/archive/sendAnswerDateService';
import { Router } from '@angular/router';

@Component({
    selector:'QuestionDetailComponent',
    templateUrl:"./questionDetail.component.html"
})
export class QuestionDetailComponent implements OnInit {

  @Input() ques?: GetArchiveQuestionDto;

  constructor(private router: Router,
    private _sendAnswerDate: SendAnswerDateService) { }

  ngOnInit(): void {
    this.AddDto.questionId = this.ques?.id;
  }

  Errors: string[] = [];
  dateValue = new FormControl();
  AddDto: SendAnswerDateDto = {
  };


  selectDate() {
    this.AddDto.questionId = this.ques?.id;
    this._sendAnswerDate.AddProcess(this.AddDto).subscribe(p => {
      if (p.isSuccess)
        this.router.navigate(["/Archive"]);
      else
        for (let a of p.message.split('|'))
          this.Errors.push(a);
    }, (error) => {
      this.Errors = [];
      for (let a of error.error.message.split('|'))
        this.Errors.push(a);
    })
  }
}
