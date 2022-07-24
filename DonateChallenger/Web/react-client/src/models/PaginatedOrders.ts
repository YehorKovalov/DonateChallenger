export interface PaginatedOrders<TData> {
     totalCount: number,
     totalPages: number,
     challengeOrdersPerPage: number,
     currentPage: number,
     data: TData[]
}