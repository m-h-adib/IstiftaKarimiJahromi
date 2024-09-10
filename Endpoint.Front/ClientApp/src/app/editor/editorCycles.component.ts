import { Component, OnInit } from '@angular/core';
import { GetProcessCycleRequestDto } from '../_models/process/getProcessCycleRequestDto';
import { GetProcessCyclesDto } from '../_models/process/getProcessCyclesDto';
import { GetProcessCyclesService } from '../_services/process/getProcessCyclesService';
import { ActivatedRoute, Router } from '@angular/router';
import { AddProcessQuestionDto } from '../_models/process/addProcessQuestionDto';
import { SearchQuestionRequestDto } from '../_models/process/searchQuestionRequestDto';
import { GetQuestionCategoryDto } from '../_models/questionCategory/GetQuestionCategoryDto';
import { GetQuestionCategoriesService } from '../_services/questionCategory/getQuestionCategoriesService';
import { AddProcessQuestionService } from '../_services/process/addProcessQuestionService';
import { AcceptOrRejectProcessDto } from '../_models/process/acceptOrRejectProcessDto';
import { AcceptOrRejectProcessService } from '../_services/process/acceptOrRejectProcessService';
import { KeyValueType } from '../_models/common/keyValueType';
import { GetUsersByRolesService } from '../_services/user/getUsersByRolesService';
import { GetResearchCycleRequestDto } from '../_models/research/getResearchCycleRequestDto';
import { AddResearchQuestionDto } from '../_models/research/addResearchQuestionDto';
import { GetResearchCyclesDto } from '../_models/research/getResearchCyclesDto';
import { AcceptOrRejectResearchDto } from '../_models/research/acceptOrRejectResearchDto';
import { GetResearchCyclesService } from '../_services/research/getResearchCyclesService';
import { AddResearchQuestionService } from '../_services/research/addResearchQuestionService';
import { AcceptOrRejectResearchService } from '../_services/research/acceptOrRejectResearchService';
import { GetEditorCycleRequestDto } from '../_models/editor/getEditorCycleRequestDto';
import { AddEditorQuestionDto } from '../_models/editor/addEditorQuestionDto';
import { GetEditorCycleDto } from '../_models/editor/getEditorCycleDto';
import { AcceptOrRejectEditorDto } from '../_models/editor/acceptOrRejectEditorDto';
import { GetEditorCyclesService } from '../_services/editor/getEditorCyclesService';
import { AddEditorQuestionService } from '../_services/editor/addResearchQuestionService';
import { AcceptOrRejectEditorService } from '../_services/editor/acceptOrRejectResearchService';
import { GetLanguageDto } from '../_models/language/GetLanguageDto';
import { GetLanguagesService } from '../_services/language/getLanguagesService';

@Component({
    templateUrl: './editorCycles.component.html',
  selector: 'EditorCyclesComponent'
  })
  
export class EditorCyclesComponent implements OnInit {
  
  rejectDialog?: boolean = false;
  referenceDialog?: boolean = false;
  access: string[] = [];
  langs:GetLanguageDto[]=[];
  refrenceUsers: KeyValueType[] = [];
  refrenceUsers1: KeyValueType[] = [];
    req: GetEditorCycleRequestDto = {};
  add: AddEditorQuestionDto = {
  };
  cycles?: GetEditorCycleDto[] = [];
 
  acceptOrRejectEditor: AcceptOrRejectEditorDto = {};
    

  constructor(private router: Router,
    private _getEditorCyclesService: GetEditorCyclesService,
    private _addEditorQuestionService: AddEditorQuestionService,
    private _acceptOrRejectEditorService: AcceptOrRejectEditorService,
    private _getUsersByRolesService: GetUsersByRolesService,
    private _getLanguagesService:GetLanguagesService,
        private route:ActivatedRoute) { }
  
    ngOnInit(): void {

        this.req.questionId = Number.parseInt(this.route.snapshot.paramMap.get('questionId')!);

      this.add.questionId = this.req.questionId;

      this.access = JSON.parse(localStorage.getItem("access")!);
  
      this.getData();
    }
  
  getData() {

    this._getUsersByRolesService.GetUsers([1, 2, 5]).subscribe(p => {
      this.refrenceUsers = p.data;
      console.log(this.refrenceUsers);
    })

    this._getUsersByRolesService.GetUsers([1, 2, 6]).subscribe(p => {
      this.refrenceUsers1 = p.data;
      console.log(this.refrenceUsers);
    })

    this._getEditorCyclesService.GetCycles(this.req!).subscribe(p => {
        this.cycles = p.data;
        console.log(p.data);
      })

      this._getLanguagesService.GetLanguages().subscribe(p=>{
        this.langs=p.data;
      })

      // this._GetQuestionCategoriesService.GetQuestionCategories().subscribe(p => {
      //   this.myTree = p.data;
      // })
    }
  
    
    getAccess(id: string): boolean {
      if (JSON.parse(localStorage.getItem("access")!).find((p: string) => p == id) != undefined)
        return true;
      else
        return false;
    }

   

  showRejectDialog(id?: number) {
   
    this.rejectDialog = !this.rejectDialog;
    this.acceptOrRejectEditor.cycleId = id;
  }
  showReferenceDialog(id?:number) {
    this.referenceDialog = !this.referenceDialog;
    this.acceptOrRejectEditor.cycleId = id;
  }
  reject() {
    this.acceptOrRejectEditor.isAccept = false;
    
    this._acceptOrRejectEditorService.AddProcess(this.acceptOrRejectEditor).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(["/editors"]);
      }
    }, (error) => {
      alert(error.error.message);
    })
  }
  reference() {
    this.acceptOrRejectEditor.isAccept = true;

    this._acceptOrRejectEditorService.AddProcess(this.acceptOrRejectEditor).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(["/editors"]);
      }
    }, (error) => {
      alert(error.error.message);
    })
  }

  Save() {
    if(this.add.languageId==null || this.add.languageId==0)
    {
      alert("لطفا زبان را انتخاب کنید");
    }
   // if (this.add.description == null || this.add.description == "") {
    //  alert("لطفا توضیحات را بنویسید");
   // }
    if (this.add.answerText == null || this.add.answerText == "") {
      alert("لطفا پاسخ را بنویسید");
    }
    else if (this.add.reasonText == null || this.add.reasonText == "") {
      alert("لطفا دلیل را بنویسید");
    }
    else {
      this._addEditorQuestionService.AddProcess(this.add).subscribe(p => {
        if (p.isSuccess)
          this.router.navigate(["/editors"]);
        else
          alert(p.message);
      }, (error) => {
        let Errors: String = "";
        for (let a of error.error.message.split('|'))
          Errors += a + "\n";

        alert(Errors);
      })
    }
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
