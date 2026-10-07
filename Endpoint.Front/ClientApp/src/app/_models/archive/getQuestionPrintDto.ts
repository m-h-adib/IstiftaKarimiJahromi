export interface GetQuestionPrintAnswerDto {
  language?: string;
  answerText?: string;
  reason?: string;
  description?: string;
  date?: string;
  editorUser?: string;
}

export interface GetQuestionPrintDto {
  id: number;
  code?: string;
  title?: string;
  questionDate?: string;
  name?: string;
  mobile?: string;
  address?: string;
  questionText?: string;
  questionCategory?: string;
  answerDate?: string;
  answers: GetQuestionPrintAnswerDto[];
}
