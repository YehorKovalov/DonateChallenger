export interface GetPaginatedCommentsResponse<TComment> {
     totalCount: number;
     totalPages: number;
     commentsPerPage: number;
     currentPage: number;
     data: TComment[];
}