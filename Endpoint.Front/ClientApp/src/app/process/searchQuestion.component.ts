import { Component, EventEmitter, Output } from "@angular/core";
import { GetQuestionCategoryDto } from '../_models/questionCategory/GetQuestionCategoryDto';
import { GetQuestionCategoriesService } from '../_services/questionCategory/getQuestionCategoriesService';
import { SearchQuestionDto } from "../_models/process/searechQuestionDto";
import { SearchQuestionsService } from "../_services/process/searchQuestionsService";
import { SearchQuestionRequestDto } from "../_models/process/searchQuestionRequestDto";
import { KeyValueType } from "../_models/common/keyValueType";

@Component({
    selector:'SearchQuestionComponent',
    templateUrl:'./searchQuestion.component.html'
})

export class SearchQuestionComponent{
    catId?:number;
    catDialog: boolean = false;
    myTree: GetQuestionCategoryDto[] = [];
  catName: string = "دسته سوال";
  items: SearchQuestionDto[]=[];
  req: SearchQuestionRequestDto = {};
  @Output() newItemEvent = new EventEmitter<KeyValueType>();
  constructor(private _GetQuestionCategoriesService: GetQuestionCategoriesService,
    private _searchQuestionService: SearchQuestionsService) { }
    
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

  search() {
    this._searchQuestionService.Search(this.req).subscribe(p => {
      //this.items.push(p.data);
      this.items = p.data;
      //console.log(this.items);
    })
  }

  select(id: number, title: string) {
    let a: KeyValueType = {};
    a.key = id;
    a.value = title;
    this.newItemEvent.emit(a);

  }
}
