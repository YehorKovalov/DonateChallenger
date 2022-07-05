export interface ChangeStreamerProfileDataResponse<TData> {
     changedData: TData;
     validationErrors: string[];
     succeeded: boolean;
}