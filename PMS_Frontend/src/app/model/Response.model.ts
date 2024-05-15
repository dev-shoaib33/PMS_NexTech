export interface BaseResponseModel {
  isSuccess: boolean;
  message: string;
  code: number;
  errorCode?: string | null;
}

export interface ResponseModel<VM> extends BaseResponseModel {
  data?: VM | null;
}

export interface ApiResponseModel<VM> extends BaseResponseModel {
  data?: VM | null;
}

export interface GridResponseModel<VM> extends BaseResponseModel {
  itemList?: VM[] | null;
  totalCount: number;
}

