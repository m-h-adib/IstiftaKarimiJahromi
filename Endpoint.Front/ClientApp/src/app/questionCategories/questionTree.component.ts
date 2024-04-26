import { Component, EventEmitter, Input, Output } from "@angular/core";
import { GetQuestionCategoryDto } from "../_models/questionCategory/GetQuestionCategoryDto";
import { DeleteQuestionCategoryService } from "../_services/questionCategory/deleteQuestionCategoryService";
import { UpdateQuestionCategoryService } from "../_services/questionCategory/updateQuestionCategoryService";
import { UpdateQuestionCategoryDto } from "../_models/questionCategory/UpdateQuestionCategoryDto";
import { StoreService } from "../StoreService";

@Component({
  selector: 'question-view',
  templateUrl: './questionTree.component.html'
})
export class QuestionTreeComponent {
  deleteDialog: boolean = false;
  updateDto?: UpdateQuestionCategoryDto = {};

  @Input({ required: true }) tree!: GetQuestionCategoryDto[];
  @Output() newItemEvent = new EventEmitter<number>();
  @Output() renew = new EventEmitter();

  constructor(private _deleteQuestionCategoryService: DeleteQuestionCategoryService,
    private _updateQuestionCategoryService: UpdateQuestionCategoryService,
    private store: StoreService) { }

  change(id: number) {

    this.store.SetQusetionCategory(id);
    
    this.newItemEvent.emit(id);
  }

  deleteQuestionCategory() {
    this._deleteQuestionCategoryService.DeleteQuestionCategory(this.store.QuestionCategorySelectItem!).subscribe(p => {
      if (p.isSuccess) {
        this.store.SetQusetionCategory(undefined);
        this.deleteDialog = !this.deleteDialog;
        this.renew.emit();
      } else {
        this.deleteDialog = !this.deleteDialog;
        this.store.SetQusetionCategory(undefined);
        alert(p.message);
      }
    })
  }

  showDeleteDialog(id?: number) {
    this.store.SetQusetionCategory(id);
    this.deleteDialog = !this.deleteDialog;
  }

  showEditDialog(id?: number) {
    this.tree.find(p => p.id == id!)!.isEditMode = true;
  }
  hideEditDialog(id?: number) {
    this.renew.emit();
    this.tree.find(p => p.id == id!)!.isEditMode = false;
  }

  GetData() {
    this.renew.emit();
  }

  Update(id?: number) {
    //alert(this.selectItem);
    //console.log(this.selectItem);
   
    var a = this.tree.find(p => p.id == id!)!;
    this.updateDto!.id = a.id;
    this.updateDto!.title = a.title;
    if (this.store.QuestionCategorySelectItem != undefined)
      this.updateDto!.parentId = this.store.QuestionCategorySelectItem;
    this.updateDto!.order = a.order;

    this._updateQuestionCategoryService.UpdateQuestionCategory(this.updateDto!).subscribe(p => {
      if (p.isSuccess) {
        this.store.SetQusetionCategory(undefined);
        this.updateDto = {};
        this.renew.emit();
      } else {
        alert(p.message);
      }
    })
  }
}

