export interface ChangeProfileDataResponse<TData> {
     changedData: TData;
     validationErrors: string[];
     succeeded: boolean;
}