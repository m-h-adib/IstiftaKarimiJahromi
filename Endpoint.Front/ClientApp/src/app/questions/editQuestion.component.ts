import { Component, OnInit } from "@angular/core";
import { AddQuestionDto } from "../_models/question/addQuestionDto";
import { GetProvincesService } from "../_services/province/getProvincesService";
import { GetCountriesService } from "../_services/country/getCountriesService";
import { GetCountryDto } from "../_models/country/GetCountryDto";
import { GetProvinceDto } from "../_models/province/GetProvinceDto";
import { GetJobCategoryDto } from "../_models/jobCategory/GetJobCategoryDto";
import { GetJobCategoriesService } from "../_services/jobCategory/getJobCategoriesService";
import { GetMethodOfRecieveQuestionsService } from "../_services/methodOfRecieveQuestion/getMethodOfRecieveQuestionsService";
import { GetMethodOfRecieveQuestionDto } from "../_models/methodRecieveQuestion/GetMethodOfRecieveQuestionDto";
import { FormControl } from "@angular/forms";
import { GetQuestionCategoryDto } from "../_models/questionCategory/GetQuestionCategoryDto";
import { GetQuestionCategoriesService } from "../_services/questionCategory/getQuestionCategoriesService";
import { AddQuestionService } from "../_services/question/addQuestionService";
import { ActivatedRoute, Router } from "@angular/router";
import { UpdateQuestionDto } from "../_models/question/updateQuestionDto";
import { UpdateQuestionService } from "../_services/question/updateQuestionService";
import { GetOneQuestionService } from "../_services/question/getOneQuestionService";
import { GetOneQuestionDto } from "../_models/question/getOneQuestionDto";
import { DeleteQuestionService } from "../_services/question/deleteQuestionService";
import { DeleteQuestionFileService } from "../_services/question/deleteQuestionFileService";

@Component({
  templateUrl: './editQuestion.component.html',
  selector: 'EditQuestionComponent'
})
export class EditQuestionComponent implements OnInit {
  Errors: string[] = [];
  AddDto: UpdateQuestionDto = {
    attachs: []
  };
  id?: string;
  GetOne: GetOneQuestionDto = {};
  IsLoading: boolean = false;
  countries: GetCountryDto[] = [];
  provinces: GetProvinceDto[] = [];
  jobs: GetJobCategoryDto[] = [];
  methods: GetMethodOfRecieveQuestionDto[] = [];
  dateValue = new FormControl();
  catDialog: boolean = false;
  myTree: GetQuestionCategoryDto[] = [];
  myFilterTree: GetQuestionCategoryDto[] = [];
  catName: string = "دسته سوال";
  catId?: number;
  deleteItem?: number;
  deleteDialog?: boolean = false;
  

  constructor(private router: Router,
    private _getCountriesService: GetCountriesService,
    private _getProvincesService: GetProvincesService,
    private _getJobCatergoriesService: GetJobCategoriesService,
    private _getMethodOfReceiveQuestionsService: GetMethodOfRecieveQuestionsService,
    private _GetQuestionCategoriesService: GetQuestionCategoriesService,
    private _updateQuestionService: UpdateQuestionService,
    private _getOneQuestionService: GetOneQuestionService,
    private _deleteQuestionFileService: DeleteQuestionFileService,
    private route: ActivatedRoute) { }


  ngOnInit(): void {

    this.id = this.route.snapshot.paramMap.get('id')!;

    this._getCountriesService.GetUsers().subscribe(p => {
      this.countries = p.data;

    });

    this._getJobCatergoriesService.GetUsers().subscribe(p => {
      this.jobs = p.data;
    });

    this._getMethodOfReceiveQuestionsService.GetMethodOfRecieveQuestions().subscribe(p => {
      this.methods = p.data;
    });

    this._GetQuestionCategoriesService.GetQuestionCategories().subscribe(p => {
      this.myTree = p.data;
      this.myFilterTree = p.data;

      this._getOneQuestionService.GetOneQuestion(Number.parseInt(this.id!)).subscribe(p => {
        this.GetOne = p.data;
        console.log(this.GetOne);
        this.AddDto = this.GetOne as UpdateQuestionDto;
       // this.AddDto.attachs = [];
        
        this.changeCountry();
        this.catId = this.AddDto.questionCategoryId;
        this.findInTree(this.myTree, this.catId!);
        console.log(this.AddDto);
        
      })


    })

    
  }

  changeCountry() {
    console.log(this.AddDto.countryId);
    if (this.AddDto.countryId != undefined && this.AddDto.countryId != null) {
      this._getProvincesService.GetProvinces(this.AddDto.countryId).subscribe(p => {
        this.provinces = p.data;
      })
    }
    else {
      this.provinces = [];
    }
  }

 
  showCatDialog() {
    this.catDialog = !this.catDialog;
  }

  selectCat(id: number) {
    this.catId = id;
  }

  selectCategory() {
    console.log(this.catId);
    if (this.catId != undefined) {
      this.AddDto.questionCategoryId = this.catId;
      this.findInTree(this.myTree, this.catId);
    }
    this.catDialog = !this.catDialog;
  }

  findInTree(tree: GetQuestionCategoryDto[], id: number) {
    for (var i = 0; i < tree.length; i++) {
      if (tree[i].id == id) {
        tree[i].isSelect = true;
        this.catName = tree[i].title!;
        break;
      }
      if (tree[i].childs?.length! > 0)
        this.findInTree(tree[i].childs!, id);
    }
  }
  onSearchChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    console.log(value);
    this.myFilterTree = this.searchTree(this.myTree, value);
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

      const selfMatch = title.indexOf(term) !== -1;

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

  Save() {
    //console.log(this.AddDto);
    this._updateQuestionService.UpdateQuestion(this.AddDto).subscribe(p => {
      if (p.isSuccess)
        this.router.navigate(["/questions"]);
      else
        for (let a of p.message.split('|'))
          this.Errors.push(a);
    }, (error) => {
      this.Errors = [];
      for (let a of error.error.message.split('|'))
        this.Errors.push(a);
    })
  }

  onFilechange(event: any) {
    //console.log(event.target.files[0])
    for (var i = 0; i < event.target.files.length; i++) {
      this.AddDto.attachs?.push(event.target.files[i]);
    }
    
    
  }

  deleteFile() {
    this._deleteQuestionFileService.DeleteQuestionFile(this.deleteItem).subscribe(p => {
      console.log(this.GetOne.attachs?.filter(p => p.id == this.deleteItem));
      const index = this.GetOne.attachs?.findIndex(p => p.id == this.deleteItem)!;
      this.GetOne.attachs?.splice(index, 1);
      this.deleteItem = undefined;
      this.deleteDialog = false;
    })
  }

  showDeleteDialog(id?: number) {
    this.deleteItem = id;
    this.deleteDialog = !this.deleteDialog;
  }
}
