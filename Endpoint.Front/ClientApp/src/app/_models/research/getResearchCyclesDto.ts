export interface GetResearchCyclesDto{
    id?:number;
    researchId?:number;
    qDesctiption?:string;
    pDescription?:string;
    duplicateId?:number;
    duplicate?:string;
    isAccept?:boolean;
    hasProccess?:boolean;
  name?: string;
  answerText?: string;
  reasonText?: string;
  answerFile?: string;
  reasonFile?: string;
}
