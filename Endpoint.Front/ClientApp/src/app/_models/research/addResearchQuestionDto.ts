export interface AddResearchQuestionDto{
    questionId?:number;
    description?:string;
  answerText?: string;
  reasonText?: string;
  answerFile?: File[];
  reasonFile?: File[];
  answerFile1?: string;
  reasonFile1?: string;
}
