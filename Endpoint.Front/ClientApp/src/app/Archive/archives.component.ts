import { Component, OnInit } from '@angular/core';
import { GetArchiveQuestionRequestDto } from '../_models/archive/getArchiveQuestionRequestDto';
import { GetQuestionCategoryDto } from '../_models/questionCategory/GetQuestionCategoryDto';
import { GetQuestionCategoriesService } from '../_services/questionCategory/getQuestionCategoriesService';
import { FormControl } from '@angular/forms';
import { GetArchiveQuestionService } from '../_services/archive/getArchiveQuestionsService';
import { GetArchiveQuestionDto } from '../_models/archive/getArchiveQuestionDto';
import { GetCountryDto } from '../_models/country/GetCountryDto';
import { GetCountriesService } from '../_services/country/getCountriesService';
import { GetProvinceDto } from '../_models/province/GetProvinceDto';
import { GetProvincesService } from '../_services/province/getProvincesService';
import { GetJobCategoryDto } from '../_models/jobCategory/GetJobCategoryDto';
import { GetJobCategoriesService } from '../_services/jobCategory/getJobCategoriesService';
import { GetMethodOfRecieveQuestionDto } from '../_models/methodRecieveQuestion/GetMethodOfRecieveQuestionDto';
import { GetMethodOfRecieveQuestionsService } from '../_services/methodOfRecieveQuestion/getMethodOfRecieveQuestionsService';
import { GetArchiveStepsService } from '../_services/archive/getArchiveStepsService';
import { GetArchiveStepDto } from '../_models/archive/getArchiveStepDto';
import { GetArchiveResultService } from '../_services/archive/getArchiveResultService';
import { GetArchiveResultDto } from '../_models/archive/getArchiveResultDto';
import { DeleteArchiveQuestionService } from '../_services/archive/deleteArchiveQuestionService';

@Component({
    templateUrl:'./archives.component.html',
    selector: 'AdminsComponent'
})
export class ArchivesComponent implements OnInit{
   
    Filter: GetArchiveQuestionRequestDto = {
        PageNumber:1,
        PageSize:15,
        Title:"",
        City:"",
        FirstName:"",
        FromDate:"",
        LastName:"",
        Mobile:"",
        ToDate:""
    };
    pageCount: number = 1;
    catName: string = "دسته سوال";
    catDialog: boolean = false;
    detailDialog: boolean = false;
    stepDialog:boolean=false;
    resultDialog:boolean=false;
    myTree: GetQuestionCategoryDto[] = [];
    myFilterTree: GetQuestionCategoryDto[] = [];
    catId?: number;
    dateValue = new FormControl();
    dateValue2 = new FormControl();
    Questions:GetArchiveQuestionDto[]=[];
    Countries:GetCountryDto[]=[];
    Provinces:GetProvinceDto[]=[];
    Jobs:GetJobCategoryDto[]=[];
    Methods:GetMethodOfRecieveQuestionDto[]=[];
    SelectQues:GetArchiveQuestionDto={
      title:"",
      address:"",
      city:"",
      country:"",
      education:"",
      email:"",
      gender:"",
      job:"",
      jobCategory:"",
      methodOfReceiveQuestion:"",
      mobile:"",
      name:"",
      province:"",
      questionCategory:"",
      questionDate:"",
      id:1
    };
    Steps:GetArchiveStepDto[]=[];
  Results: GetArchiveResultDto[] = [];
  access: string[] = [];
  selectArchive?: number;
  deleteDialog: boolean = false;
  

    constructor(private _GetQuestionCategoriesService:GetQuestionCategoriesService,
        private _GetArchivesService:GetArchiveQuestionService,
        private _GetCountriesService:GetCountriesService,
        private _GetProvincesService:GetProvincesService,
        private _GetJobsService:GetJobCategoriesService,
        private _GetMethodsService:GetMethodOfRecieveQuestionsService,
        private _GetArchiveSteps:GetArchiveStepsService,
      private _GetArchiveResults: GetArchiveResultService,
      private _DeleteArchiveQuestion: DeleteArchiveQuestionService) {

    }

    deleteCat(){
      this.catId=undefined;
      this.catName = "دسته سوال";
      this.Filter.QuestionCategoryId = undefined;
    }

  showDeleteDialog(id?: number) {
    this.selectArchive = id;
    this.deleteDialog = !this.deleteDialog;
  }

  deleteArchive() {
    this._DeleteArchiveQuestion.Delete(this.selectArchive!).subscribe(p => {
      if (p.isSuccess) {
        this.Questions! = this.Questions!.filter(p => p.id != this.selectArchive);
        this.showDeleteDialog();

      }
    })
  }

  getAccess(id: string): boolean {
    if (JSON.parse(localStorage.getItem("access")!).find((p: string) => p == id) != undefined)
      return true;
    else
      return false;
  }

  onSearchChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    console.log(value);
    this.myFilterTree = this.searchTree(this.myTree, value);
  }

  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);

        this._GetQuestionCategoriesService.GetQuestionCategories().subscribe(p => {
          this.myTree = p.data;
          this.myFilterTree = p.data;
        })

  
       this.getData();
          this._GetCountriesService.GetUsers().subscribe(p=>{
            this.Countries=p.data;
 
        })

        this._GetJobsService.GetUsers().subscribe(p=>{
            this.Jobs=p.data;
        })
        this._GetMethodsService.GetMethodOfRecieveQuestions().subscribe(p=>{
            this.Methods=p.data;
        })
    }

    getData() {
      this._GetArchivesService.GetArchives(this.Filter!).subscribe(p=>{
        this.Questions=p.data.items;
        this.pageCount = Math.floor((p.data.paging.totalItems % this.Filter.PageSize!) == 0 ? (p.data.paging.totalItems / this.Filter.PageSize!) : (p.data.paging.totalItems / this.Filter.PageSize!) + 1)
    })
    }

    inc() {
      this.Filter.PageNumber! += 1;
      this.getData();
    }
    dec() {
      this.Filter.PageNumber! -= 1;
      this.getData();
    }
  
    change() {
      this.getData();
    }

    changeCountry() {
        if (this.Filter.CountryId != undefined && this.Filter.CountryId != null) {
          this._GetProvincesService.GetProvinces(this.Filter.CountryId).subscribe(p => {
            this.Provinces = p.data;
          })
        }
        else {
          this.Provinces = [];
        }
      }

  search(): void{
    this.Filter.PageNumber = 1;
        this._GetArchivesService.GetArchives(this.Filter!).subscribe(p=>{
          this.Questions = p.data.items;
          this.pageCount = Math.floor((p.data.paging.totalItems % this.Filter.PageSize!) == 0 ? (p.data.paging.totalItems / this.Filter.PageSize!) : (p.data.paging.totalItems / this.Filter.PageSize!) + 1)
        })
    }

    showCatDialog() {
        this.catDialog = !this.catDialog;
    }

    selectCat(id: number) {
        this.catId = id;
      }
      showDetailDialog(id?:number){
        if(id!=null && id!=undefined){
          this.SelectQues=this.Questions[this.Questions.findIndex(p=>p.id==id)];
        }
        
        this.detailDialog=!this.detailDialog;
      }
      selectCategory() {
        console.log(this.catId);
        if (this.catId != undefined) {
          this.Filter.QuestionCategoryId = this.catId;
          this.findInTree(this.myTree, this.catId);
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

      showStepDialog(id?:number):void{
        if(id!=null){
        this._GetArchiveSteps.GetSteps(id).subscribe(p=>{
          this.Steps=p.data;
          this.stepDialog=!this.stepDialog;
        })
      }
      else{
        this.stepDialog=!this.stepDialog;
      }
    }


    showResultDialog(id?:number):void{
      if(id!=null){
      this._GetArchiveResults.GetResults(id).subscribe(p=>{
        this.Results=p.data;
        this.resultDialog=!this.resultDialog;
      })
    }
    else{
      this.resultDialog=!this.resultDialog;
    }
  }


  searchTree(
    tree: GetQuestionCategoryDto[],
    search: string
  ): GetQuestionCategoryDto[] {

    const term = search.trim().toLowerCase();

    // اگر سرچ خالی بود همون کل درخت رو بده
    if (!term) return tree;

    const filterNode = (
      node: GetQuestionCategoryDto
    ): GetQuestionCategoryDto | null => {

      const title = node.title?.toLowerCase() ?? "";

      const selfMatch = title.includes(term);

      const filteredChildren =
        (node.childs ?? [])
          .map(child => filterNode(child))
          .filter((x): x is GetQuestionCategoryDto => x !== null);

      const hasChildMatch = filteredChildren.length > 0;

      // اگر خود node یا بچه‌ها match داشت
      if (selfMatch || hasChildMatch) {
        return {
          ...node,
          // فقط اگر بچه دارد expand کن
          expand: hasChildMatch,
          childs: filteredChildren
        };
      }

      return null;
    };

    return tree
      .map(node => filterNode(node))
      .filter((x): x is GetQuestionCategoryDto => x !== null);
  }
}
