import { Component, OnInit } from "@angular/core";
import { GetQuestionCategoryDto } from "../_models/questionCategory/GetQuestionCategoryDto";
import { GetQuestionCategoriesService } from "../_services/questionCategory/getQuestionCategoriesService";
import { AddQuestionCategoryService } from "../_services/questionCategory/addQuestionCategoryService";
import { AddQuestionCategoryDto } from "../_models/questionCategory/AddQuestionCategoryDto";

@Component({
  templateUrl: './questionCategories.component.html',
  selector:'QuestionCategoriesComponent'
})
export class QuestionCategoriesComponent implements OnInit {
  myTree: GetQuestionCategoryDto[] = [];
  dto: AddQuestionCategoryDto = {};
  access: string[] = [];
 
  constructor(private _getQuestionCategoriesService: GetQuestionCategoriesService,
    private _addQuestionCategoryService: AddQuestionCategoryService) { }


  ngOnInit(): void {
    this.access = JSON.parse(localStorage.getItem("access")!);
    if (this.access.find(p => p == "QuestionCategories") != undefined) {
      this.GetData();
    }
    
    
  };

  GetData() {
    this._getQuestionCategoriesService.GetQuestionCategories().subscribe(p => {
      this.myTree = p.data;
      
    })
  }

  Add() {
    console.log(this.dto);
    this._addQuestionCategoryService.AddQuestionCategory(this.dto).subscribe(p => {
      if (p.isSuccess) {
        this.dto = {};
        this.GetData();
      } else {
        alert(p.message);
      }
    })
  }


  change(id: number) {
    this.dto.parentId = id;
  }


  getAccess(id: string): boolean {
    if (this.access.find(p => p == id) != undefined)
      return true;
    else
      return false;
  }
}
