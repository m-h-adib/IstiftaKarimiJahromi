import { Component, OnInit } from '@angular/core';
import { GetProcessCycleRequestDto } from '../_models/process/getProcessCycleRequestDto';
import { GetProcessCyclesDto } from '../_models/process/getProcessCyclesDto';
import { GetProcessCyclesService } from '../_services/process/getProcessCyclesService';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AddProcessQuestionDto } from '../_models/process/addProcessQuestionDto';
import { SearchQuestionRequestDto } from '../_models/process/searchQuestionRequestDto';
import { GetQuestionCategoryDto } from '../_models/questionCategory/GetQuestionCategoryDto';
import { GetQuestionCategoriesService } from '../_services/questionCategory/getQuestionCategoriesService';
import { AddProcessQuestionService } from '../_services/process/addProcessQuestionService';
import { AcceptOrRejectProcessDto } from '../_models/process/acceptOrRejectProcessDto';
import { AcceptOrRejectProcessService } from '../_services/process/acceptOrRejectProcessService';
import { KeyValueType } from '../_models/common/keyValueType';
import { GetUsersByRolesService } from '../_services/user/getUsersByRolesService';

@Component({
    templateUrl: './processCycles.component.html',
    selector: 'ProcessCyclesComponent'
  })
  
  export class ProcessCyclesComponent implements OnInit {

  access: string[] = [];

  rejectDialog?: boolean = false;
  referenceDialog?: boolean = false;
  
  refrenceUsers: KeyValueType[] = [];
  refrenceUsers1: KeyValueType[] = [];
    req: GetProcessCycleRequestDto = {};
    add:AddProcessQuestionDto={};
  cycles?: GetProcessCyclesDto[] = [];
  searchReq: SearchQuestionRequestDto = {};
  acceptOrRejectProcess: AcceptOrRejectProcessDto = {};
    
    qName?:string ='بدون سوال تکراری';
  searchDialog?: boolean = false;
  
  constructor(private _getProcessCyclesService: GetProcessCyclesService,
    private _addProccessQuestionService: AddProcessQuestionService,
    private _acceptOrRejectProcessService: AcceptOrRejectProcessService,
    private _getUsersByRolesService: GetUsersByRolesService,
    private router: Router,
        private route:ActivatedRoute) { }
  
    ngOnInit(): void {
      this.access = JSON.parse(localStorage.getItem("access")!);

      if (!this.getAccess("AddProcessQuestion"))
        this.router.navigate(["./"]);

        this.req.questionId = Number.parseInt(this.route.snapshot.paramMap.get('questionId')!);

      this.add.questionId = this.req.questionId;

      this.getData();
    }


  getData() {

    this._getUsersByRolesService.GetUsers([1, 2, 4]).subscribe(p => {
      this.refrenceUsers = p.data;
      console.log(this.refrenceUsers);
    })

    this._getUsersByRolesService.GetUsers([1, 2, 5]).subscribe(p => {
      this.refrenceUsers1 = p.data;
      console.log(this.refrenceUsers);
    })

      this._getProcessCyclesService.GetCycles(this.req!).subscribe(p => {
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

    showSearchDialog(){
      this.searchDialog = !this.searchDialog;

    }

  showRejectDialog(id?: number) {
   
    this.rejectDialog = !this.rejectDialog;
    this.acceptOrRejectProcess.cycleId = id;
  }
  showReferenceDialog(id?:number) {
    this.referenceDialog = !this.referenceDialog;
    this.acceptOrRejectProcess.cycleId = id;
  }
  reject() {
    this.acceptOrRejectProcess.isAccept = false;
    
    this._acceptOrRejectProcessService.AddProcess(this.acceptOrRejectProcess).subscribe(p => {
      if (p.isSuccess) {
        this.showRejectDialog();
        this.getData();
      }
    }, (error) => {
      alert(error.error.message);
    })
  }
  reference() {
    this.acceptOrRejectProcess.isAccept = true;

    this._acceptOrRejectProcessService.AddProcess(this.acceptOrRejectProcess).subscribe(p => {
      if (p.isSuccess) {
        this.showReferenceDialog();
        this.getData();
      }
    }, (error) => {
      alert(error.error.message);
    })
  }

  Save() {
    //if (this.add.description == null || this.add.description == "")
     // alert("لطفا توضیحات را بنویسید");

    this._addProccessQuestionService.AddProcess(this.add).subscribe(p => {
      if (p.isSuccess)
        this.getData();
      else
        alert(p.message);
    }, (error) => {
      let Errors: String = "";
      for (let a of error.error.message.split('|'))
        Errors += a + "\n";

      alert(Errors);
    })
  }

  selectQues(a: KeyValueType) {
    this.add.duplicateId = a.key;
    this.qName = a.value;
    this.showSearchDialog();
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
