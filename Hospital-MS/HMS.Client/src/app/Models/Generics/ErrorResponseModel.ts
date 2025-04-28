
export interface ErrorResponseModel<T> {
    isSuccess: boolean | null;
    message: string | null;
    errorCode: Status | null;
    results: T | null;
}


export enum Status {
    Success = 200,
    Failed = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409
}