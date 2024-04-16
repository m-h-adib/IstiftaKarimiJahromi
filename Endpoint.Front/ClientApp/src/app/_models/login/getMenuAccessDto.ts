export interface GetMenuAccessDto {
  id?: number;
  name?: string;
  title?: string;
  link?: string;
  queryParams?: string;
  childs?: GetMenuAccessDto[];
}
