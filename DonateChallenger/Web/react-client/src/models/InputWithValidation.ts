export interface InputWithValidation<TInputType> {
     errors: string[];
     state: TInputType;
}