export interface GetPaginatedCommentsRequest {
     currentPage: number;
     commentsPerPage: number;
     challengeId: number;
     userId?: string;
}