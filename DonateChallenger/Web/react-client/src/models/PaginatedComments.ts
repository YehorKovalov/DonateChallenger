export interface PaginatedComments<TData> {
     totalCount: number,
     totalPages: number,
     commentsPerPage: number,
     currentPage: number,
     data: TData[]
}