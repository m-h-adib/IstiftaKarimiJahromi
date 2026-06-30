

export interface GetArchiveQuestionRequestDto{
    
    PageNumber:number;
    PageSize:number;
    Title?:string;
    FromDate?:string;
    ToDate?:string;
    FirstName?:string;
    LastName?:string;
    Gender?:number;
    FromAge?:number;
    ToAge?:number;
    CountryId?:number;
    ProvinceId?:number;
    City?:string;
    Education?:number;
    JobCategoryId?:number;
    Mobile?:string;
    MethodOfReceiveQuestionId?:number;
  QuestionCategoryId?: number;
  Id?: string;
}
