export interface AddResearchQuestionDto{
    questionId?:number;
    description?:string;
  answerText?: string;
  reasonText?: string;
  answerFile?: File[];
  ReasonFile?: File[];
}
