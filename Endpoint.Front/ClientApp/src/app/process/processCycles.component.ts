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
  
    catDialog: boolean = false;
    access: string[] = [];
    catName?:string="دسته سوال";
    catId?: number;
    req: GetProcessCycleRequestDto = {};
    add:AddProcessQuestionDto={};
    searchReq:SearchQuestionRequestDto={};
    myTree: GetQuestionCategoryDto[] = [];
    cycles?:  GetProcessCyclesDto[] = [];
  
    constructor(private _getProcessCyclesService: GetProcessCyclesService,
        private route:ActivatedRoute,
        private _GetQuestionCategoriesService: GetQuestionCategoriesService,) { }
  
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

      this._GetQuestionCategoriesService.GetQuestionCategories().subscribe(p => {
        this.myTree = p.data;
      })
    }
  
    
    getAccess(id: string): boolean {
      if (JSON.parse(localStorage.getItem("access")!).find((p: string) => p == id) != undefined)
        return true;
      else
        return false;
    }
  
    showCatDialog(){
        this.catDialog = !this.catDialog;
    }
  
    selectCat(id: number) {
        this.catId = id;
      }
    
      selectCategory() {
        console.log( this.catId);
        if ( this.catId != undefined) {
          this.searchReq.category = this.catId;
          this.findInTree(this.myTree, this.catId!);
        }
        this.catDialog = !this.catDialog;
      }

      findInTree(tree: GetQuestionCategoryDto[], id: number) {
        for (var i = 0; i < tree.length; i++) {
          if (tree[i].id == id) {
            this.catName = tree[i].title!;
            break;
          }
          if (tree[i].childs?.length! > 0)
            this.findInTree(tree[i].childs!, id);
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
  