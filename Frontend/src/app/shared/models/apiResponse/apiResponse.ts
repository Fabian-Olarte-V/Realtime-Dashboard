export interface ApiResponse<T>{
    success: boolean;
    statusCode: number;
    message: string;
    serverTime: string;
    data: T;
}

export interface ApiHttpError {
    success: boolean;
    statusCode: number;
    message: string;
    errors?: string[];
}