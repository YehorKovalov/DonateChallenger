export interface UpdateChallengeRequest {
     challengeId: number;
     title?: string;
     description: string;
     donatePrice: number;
     donateFrom: string;
}