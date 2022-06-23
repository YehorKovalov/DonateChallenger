export interface PaginatedChallenges<TData> {
     totalCount: number,
     totalPages: number,
     challengesPerPage: number,
     currentPage: number,
     data: TData[]
}