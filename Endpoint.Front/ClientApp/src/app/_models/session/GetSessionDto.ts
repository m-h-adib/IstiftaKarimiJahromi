export interface GetSessionDto{
    id:number;
    title?:string;
    date?:string;
    time?:string;
  question?: string;
  questionId?: number;
    users?:string[];
    description?:string;
}
