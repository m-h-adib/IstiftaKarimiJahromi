export interface GetOneQuestionDto {
  id?: number;
  title?: string;
  questionDate?: string;
  firstName?: string;
  lastName?: string;
  gender?: number;
  age?: number;
  countryId?: number;
  provinceId?: number;
  city?: string;
  education?: number;
  jobCategoryId?: number;
  job?: string;
  mobile?: string;
  email?: string;
  address?: string;
  methodOfReceiveQuestionId?: number;
  questionCategoryId?: number;
  questionText?: string;
  attachs?: GetOneQuestionFileDto[];
}
export interface GetOneQuestionFileDto {
  id?: number;
  title?: string;
}
