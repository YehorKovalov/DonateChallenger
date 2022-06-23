export interface GetPaginatedStreamerChallengesResponse<TData> {
     totalCount: number,
     totalPages: number,
     challengesPerPage: number,
     currentPage: number,
     data: TData[]
}