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
  myFilterTree: GetQuestionCategoryDto[] = [];
  catName: string = "دسته سوال";
  items: SearchQuestionDto[]=[];
  req: SearchQuestionRequestDto = {
    title: ""
  };
  @Output() newItemEvent = new EventEmitter<KeyValueType>();
  constructor(private _GetQuestionCategoriesService: GetQuestionCategoriesService,
    private _searchQuestionService: SearchQuestionsService) { }


  onSearchChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    console.log(value);
    this.myFilterTree = this.searchTree(this.myTree, value);
  }
    ngOnInit(): void {
        this._GetQuestionCategoriesService.GetQuestionCategories().subscribe(p => {
          this.myTree = p.data;
          this.myFilterTree = p.data;
        })
      }

  deleteCat() {
    this.catId = undefined;
    this.catName = "دسته سوال";
    this.req.category = undefined;
  }

      showCatDialog() {
        this.catDialog = !this.catDialog;
      }
    
      selectCat(id: number) {
        this.catId = id;
        this.req.category=id;
      }
    
      selectCategory() {
        if (this.catId != undefined) {

          this.findInTree(this.myFilterTree, this.catId);
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
