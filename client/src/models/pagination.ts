// Pagination model
export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

// Class for creating paginated results that contain the data and pagination information
export class PaginatedResult<T> {
  data: T;
  pagination: Pagination;

  constructor(data: T, pagination: Pagination) {
    this.data = data;
    this.pagination = pagination;
  }
}

// Create paging params based on passed in page number and size
export class PagingParams {
  pageNumber;
  pageSize;

  constructor(pageNumber = 1, pageSize = 2) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
  }
}
