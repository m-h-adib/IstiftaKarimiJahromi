import { GetEditorCycleItemDto } from './getEditorCycleItemDto';

export interface GetEditorCycleDto {
  id?: number;
  duplicateId?: number;
  duplicate?: string;
  qDesctiption?: string;
  hasProccess?: boolean;
  isAccept?: boolean;
  name?: string;
  items?: GetEditorCycleItemDto[];
}
