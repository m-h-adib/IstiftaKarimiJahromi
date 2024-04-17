export interface GetUserAccessDto {
  id?: number;
  name?: string;
  title?: string;
  isAccess?: boolean;
  childs?: GetUserAccessDto[];
}
