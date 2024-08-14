import { Component, Input } from '@angular/core';
import { GetArchiveResultDto } from '../_models/archive/getArchiveResultDto';

@Component({
    selector:"QuestionResultComponent",
    templateUrl:"./questionResult.component.html"
})
export class QuestionResultComponent{
@Input() results:GetArchiveResultDto[]=[];
}