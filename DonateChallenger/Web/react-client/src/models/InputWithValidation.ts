export interface InputWithValidation<TInputType = string | number> {
     errors: string[];
     state: TInputType;
}