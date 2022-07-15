export interface AddChallengeOrderResponse<TId> {
    succeeded: boolean;
    errorMessages?: string[];
    orderId?: TId;
}