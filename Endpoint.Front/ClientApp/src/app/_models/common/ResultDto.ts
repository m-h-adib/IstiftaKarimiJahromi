export interface ResultDto {
  isSuccess: boolean;
  message: string;
  statusCode: number;
}

export interface DataResultDto<T> {
  data: T;
  isSuccess: boolean;
  message: string;
  statusCode: number;
}

export interface ListDataResultDto<T> {
  data: T[];
  isSuccess: boolean;
  message: string;
  statusCode: number;
}

export interface PagingDataResultDto<T> {
  data: DataResultDto1<T>,
  isSuccess: boolean;
  message: string;
  statusCode: number;
}

export interface DataResultDto1<T> {
  items: T[];
  paging: Page
}
export interface Page {
  page: number;
  size: number;
  totalItems: number;
}
