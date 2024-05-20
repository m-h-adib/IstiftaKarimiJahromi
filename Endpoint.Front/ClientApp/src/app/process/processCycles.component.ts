import { Component, OnInit } from '@angular/core';
import { GetProcessCycleRequestDto } from '../_models/process/getProcessCycleRequestDto';
import { GetProcessCyclesDto } from '../_models/process/getProcessCyclesDto';
import { GetProcessCyclesService } from '../_services/process/getProcessCyclesService';
import { ActivatedRoute } from '@angular/router';
import { AddProcessQuestionDto } from '../_models/process/addProcessQuestionDto';
import { SearchQuestionRequestDto } from '../_models/process/searchQuestionRequestDto';
import { GetQuestionCategoryDto } from '../_models/questionCategory/GetQuestionCategoryDto';
import { GetQuestionCategoriesService } from '../_services/questionCategory/getQuestionCategoriesService';

@Component({
    templateUrl: './processCycles.component.html',
    selector: 'ProcessCyclesComponent'
  })
  
  export class ProcessCyclesComponent implements OnInit {
  

    access: string[] = [];
    req: GetProcessCycleRequestDto = {};
    add:AddProcessQuestionDto={};
    searchReq:SearchQuestionRequestDto={};
    cycles?:  GetProcessCyclesDto[] = [];
    
    qName?:string ='بدون سوال تکراری';
    searchDialog?:boolean=false;
  
    constructor(private _getProcessCyclesService: GetProcessCyclesService,
        private route:ActivatedRoute) { }
  
    ngOnInit(): void {

        this.req.userId = Number.parseInt(this.route.snapshot.paramMap.get('userId')!);
        this.req.questionId = Number.parseInt(this.route.snapshot.paramMap.get('questionId')!);

      this.access = JSON.parse(localStorage.getItem("access")!);
  
      this.getData();
    }
  
    getData() {
      this._getProcessCyclesService.GetCycles(this.req!).subscribe(p => {
        //this.cycles = p.date;
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

    showSearchDialog(){
      this.searchDialog=!this.searchDialog;
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
  