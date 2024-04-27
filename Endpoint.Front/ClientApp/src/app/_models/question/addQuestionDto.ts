export interface AddQuestionDto {
  title?: string;
  address?: string;
  mobile?: string;
  provinceId?: number;
  age?: number;
  city?: string;
  countryId?: number;
  education?: number;
  email?: string;
  firstName?: string;
  gender?: number;
  job?: string;
  jobCategoryId?: number;
  lastName?: string;
  methodOfReceiveQuestionId?: number;
  questionCategoryId?: number;
  questionDate?: string;
  questionText?: string;
  attachs?: File[];
}
