export interface GetUserQueryRequestDto {
  userId?: number;
  fromDate?: string;
  toDate?: string;
  type?: number;
  page?: number;
  size?: number;
}
