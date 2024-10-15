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

@Component({
    templateUrl: './researchCycles.component.html',
  selector: 'ResearchCyclesComponent'
  })
  
export class ResearchCyclesComponent implements OnInit {
  
  rejectDialog?: boolean = false;
  referenceDialog?: boolean = false;
  access: string[] = [];
  refrenceUsers: KeyValueType[] = [];
  refrenceUsers1: KeyValueType[] = [];
    req: GetResearchCycleRequestDto = {};
  add: AddResearchQuestionDto = {
    description:"",
    answerText:"",
    reasonText:"",
    answerFile: [],
    reasonFile:[]
  };
  cycles?: GetResearchCyclesDto[] = [];
 
  acceptOrRejectResearch: AcceptOrRejectResearchDto = {};
    

  constructor(private router: Router,
    private _getResearchCyclesService: GetResearchCyclesService,
    private _addResearchQuestionService: AddResearchQuestionService,
    private _acceptOrRejectResearchService: AcceptOrRejectResearchService,
    private _getUsersByRolesService: GetUsersByRolesService,
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

    this._getResearchCyclesService.GetCycles(this.req!).subscribe(p => {
        this.cycles = p.data;
        console.log(p.data);
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
    this.acceptOrRejectResearch.cycleId = id;
  }
  showReferenceDialog(id?:number) {
    this.referenceDialog = !this.referenceDialog;
    this.acceptOrRejectResearch.cycleId = id;
  }
  reject() {
    this.acceptOrRejectResearch.isAccept = false;
    
    this._acceptOrRejectResearchService.AddProcess(this.acceptOrRejectResearch).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(["/researchQuestions"]);
      }
    }, (error) => {
      alert(error.error.message);
    })
  }
  reference() {
    this.acceptOrRejectResearch.isAccept = true;

    this._acceptOrRejectResearchService.AddProcess(this.acceptOrRejectResearch).subscribe(p => {
      if (p.isSuccess) {
        this.router.navigate(["/researchQuestions"]);
      }
    }, (error) => {
      alert(error.error.message);
    })
  }

  Save() {
   // if (this.add.description == null || this.add.description == "") {
     // alert("لطفا توضیحات را بنویسید");
  //  }
    if (this.add.answerText == null || this.add.answerText == "") {
      alert("لطفا پاسخ را بنویسید");
    }
    else if (this.add.reasonText == null || this.add.reasonText == "") {
      alert("لطفا دلیل را بنویسید");
    }
    else {
      this._addResearchQuestionService.AddProcess(this.add).subscribe(p => {
        if (p.isSuccess)
          this.router.navigate(["/researchQuestions"]);
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

  onFilechange(event: any) {
    console.log(event.target.files[0])
    this.add.answerFile?.push(event.target.files[0]);

  }

  onFilechange1(event: any) {
    console.log(event.target.files[0])
    this.add.reasonFile?.push(event.target.files[0]);

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
