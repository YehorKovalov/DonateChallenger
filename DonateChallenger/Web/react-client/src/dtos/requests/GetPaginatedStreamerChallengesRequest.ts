export interface GetPaginatedStreamerChallengesRequest {
     currentPage: number,
     challengesPerPage: number,
     filters: {[k: string]: number}
}