export interface GetQuestionCategoryDto {
  id?: number;
  title?: string;
  order?: number;
  childs?: GetQuestionCategoryDto[];
  isEditMode?: boolean;
  isSelect?: boolean;
  expand?: boolean;
}
