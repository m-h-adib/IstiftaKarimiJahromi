import { Component, Input } from '@angular/core';
import { AddQuestionDto } from '../_models/question/addQuestionDto';
import { GetArchiveQuestionDto } from '../_models/archive/getArchiveQuestionDto';

@Component({
    selector:'QuestionDetailComponent',
    templateUrl:"./questionDetail.component.html"
})
export class QuestionDetailComponent{
    @Input() ques?:GetArchiveQuestionDto;
}