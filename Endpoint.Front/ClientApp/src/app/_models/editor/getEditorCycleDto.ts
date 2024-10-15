import { GetEditorCycleItemDto } from './getEditorCycleItemDto';

export interface GetEditorCycleDto{
    id?:number;
    qDesctiption?:string;
    hasProccess?:boolean;
    isAccept?:boolean;
    name?:string;
    items?:GetEditorCycleItemDto[];
}