export interface GetPaginatedStreamerChallengesRequest {
     currentPage: number;
     challengesPerPage: number;
     streamerId: string;
     filters?: {[k: string]: number};
     sortBy?: {[k: string]: boolean};
}