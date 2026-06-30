import { Component, Input, OnInit } from "@angular/core";
import { GetPopupQuestionService } from "../_services/question/getPopupQuestionService";
import { GetPopupQuestionDto } from "../_models/question/getPopupQuestionDto";
import { SwalPortalTargets } from "@sweetalert2/ngx-sweetalert2";


@Component({
    templateUrl:'./questionPopup.component.html',
    selector:'QuestionPopupComponent'
})
export class QuestionPopupComponent implements OnInit{
    @Input() questionId!:number;
    AddDto:GetPopupQuestionDto={
        answer:"",
      answerFile: "",
      answerFile2: "",
        attach:"",
        category:"",
        question:"",
        reason:"",
      reasonFile: "",
      reasonFile2: "",
        title:""
    };

    constructor(public readonly swalTargets: SwalPortalTargets,
        private _getPopupQuestionService:GetPopupQuestionService){}
    ngOnInit(): void {
        this.getData();
    }



    getData(){
        this._getPopupQuestionService.GetPopup(this.questionId).subscribe(p=>{
            this.AddDto=p.data;
            console.log(p.data);
        });
    }
}
