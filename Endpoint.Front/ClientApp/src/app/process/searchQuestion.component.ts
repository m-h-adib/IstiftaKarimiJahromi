import { Component } from "@angular/core";
import { GetQuestionCategoryDto } from '../_models/questionCategory/GetQuestionCategoryDto';
import { GetQuestionCategoriesService } from '../_services/questionCategory/getQuestionCategoriesService';

@Component({
    selector:'SearchQuestionComponent',
    templateUrl:'./searchQuestion.component.html'
})

export class SearchQuestionComponent{
    term?:string;
    catId?:number;
    catDialog: boolean = false;
    myTree: GetQuestionCategoryDto[] = [];
    catName: string = "دسته سوال";

    constructor(private _GetQuestionCategoriesService: GetQuestionCategoriesService) { }
    
    ngOnInit(): void {
        this._GetQuestionCategoriesService.GetQuestionCategories().subscribe(p => {
          this.myTree = p.data;
        })
      }


      showCatDialog() {
        this.catDialog = !this.catDialog;
      }
    
      selectCat(id: number) {
        this.catId = id;
      }
    
      selectCategory() {
        if (this.catId != undefined) {
            
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
}