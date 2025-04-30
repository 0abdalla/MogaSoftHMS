import { Status } from "./ErrorResponseModel";

export interface PagedResponseModel<T> {
    results?: T | null;
    totalCount?: number;
    errorMessage?: string | null;
    isSuccess?: boolean;
    errorCode?: Status | null;
}