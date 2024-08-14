import { Component, Input } from '@angular/core';
import { GetArchiveStepDto } from '../_models/archive/getArchiveStepDto';

@Component({
    selector:"QuestionStepsComponent",
    templateUrl:"./questionStep.component.html"
})
export class QuestionStepsComponent{
    @Input() steps:GetArchiveStepDto[]=[];
}